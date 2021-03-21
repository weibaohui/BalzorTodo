using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign;
using Microsoft.AspNetCore.Components;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class TaskInfo : DrawerTemplate<TaskDto, TaskDto>
    {
        private readonly bool isLoading = false;

        private TaskDto taskDto;

        [Inject] public HttpClient Http { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        protected override async Task OnInitializedAsync()
        {
            taskDto = await Http.GetFromJsonAsync<TaskDto>($"api/Task/GetTaskDto?taskId={Options.TaskId}");
            await base.OnInitializedAsync();
        }

        private async void OnSave()
        {
            var result = await Http.PostAsJsonAsync("api/Task/SaveTask", taskDto);
            if (result.StatusCode == HttpStatusCode.OK)
                await CloseAsync(taskDto);
            else
                await MsgSvr.Error($"请求发生错误 {result.StatusCode}");
        }

        private async void OnCancel()
        {
            await CloseAsync();
        }
    }
}