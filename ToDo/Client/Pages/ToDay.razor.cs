using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign;
using Microsoft.AspNetCore.Components;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class ToDay
    {
        private bool isLoading = true;

        // 1、	列出当天的所有代办工作
        private List<TaskDto> taskDtos = new();
        [Inject] public HttpClient Http { get; set; }

        //2、	添加代办
        public MessageService MsgSrv { get; set; }

        //3、	编辑待办
        [Inject] public DrawerService DrawerSrv { get; set; }


        //6、	删除代办
        [Inject] public ConfirmService ConfirmSrv { get; set; }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            taskDtos = await Http.GetFromJsonAsync<List<TaskDto>>("api/Task/GetToDayTask");
            isLoading = false;
            await base.OnInitializedAsync();
        }

        private  void OnInsert(TaskDto item)
        {
            taskDtos.Add(item);
        }

        private async void OnCardClick(TaskDto task)
        {
            var result =
                await DrawerSrv.CreateDialogAsync<TaskInfo, TaskDto, TaskDto>(task, title: task.Title, width: 450);
            if (result == null) return;
            var index = taskDtos.FindIndex(x => x.TaskId == result.TaskId);
            taskDtos[index] = result;
            await InvokeAsync(StateHasChanged);
        }


        //4、	修改重要程度
        private async void OnStar(TaskDto task)
        {
            var req = new SetImportantReq
            {
                TaskId = task.TaskId,
                IsImportant = !task.IsImportant
            };

            var result = await Http.PostAsJsonAsync("api/Task/SetImportant", req);
            if (result.IsSuccessStatusCode)
            {
                task.IsImportant = req.IsImportant;
                StateHasChanged();
            }
        }

        //5、	修改完成状态
        private async void OnFinish(TaskDto task)
        {
            var req = new SetFinishReq
            {
                TaskId = task.TaskId,
                IsFinish = !task.IsFinish
            };

            var result = await Http.PostAsJsonAsync("api/Task/SetFinish", req);
            if (result.IsSuccessStatusCode)
            {
                task.IsFinish = req.IsFinish;
                StateHasChanged();
            }
        }

        public async Task OnDel(TaskDto task)
        {
            if (await ConfirmSrv.Show($"是否删除任务 {task.Title}", "删除", ConfirmButtons.YesNo) == ConfirmResult.Yes)
                taskDtos.Remove(task);
        }
    }
}