using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign;
using ToDo.Client.Pages;
using ToDo.Shared;

namespace ToDo.Client
{
    public class TaskDetailServices
    {
        public TaskDetailServices(DrawerService drawerSvr)
        {
            DrawerSvr = drawerSvr;
        }

        public DrawerService DrawerSvr { get; set; }

        public async Task EditTask(TaskDto taskDto, List<TaskDto> datas)
        {
            var taskItem =
                await DrawerSvr.CreateDialogAsync<TaskInfo, TaskDto, TaskDto>(taskDto, title: taskDto.Title,
                    width: 450);
            if (taskItem == null) return;
            var index = datas.FindIndex(x => x.TaskId == taskItem.TaskId);
            datas[index] = taskItem;
        }
    }
}