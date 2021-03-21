using AntDesign.Charts;
using Microsoft.AspNetCore.Components;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class TaskItem
    {
        //进度迷你图
        private readonly RingProgressConfig progressConfig = new()
        {
            Width = 30,
            Height = 30
        };

        //任务内容
        [Parameter] public TaskDto Item { get; set; }

        //完成图标事件
        [Parameter] public EventCallback<TaskDto> OnFinish { get; set; }

        //条目点击事件
        [Parameter] public EventCallback<TaskDto> OnCard { get; set; }

        //删除图标事件
        [Parameter] public EventCallback<TaskDto> OnDel { get; set; }

        //重要图标事件
        [Parameter] public EventCallback<TaskDto> OnStar { get; set; }

        //是否相似重要图标
        [Parameter] public bool ShowStar { get; set; } = true;

        //支持标题模板
        [Parameter] public RenderFragment TitleTemplate { get; set; }

        public async void OnFinishClick()
        {
            if (OnFinish.HasDelegate)
                await OnFinish.InvokeAsync(Item);
        }

        public async void OnCardClick()
        {
            if (OnCard.HasDelegate)
                await OnCard.InvokeAsync(Item);
        }

        public async void OnDelClick()
        {
            if (OnDel.HasDelegate)
                await OnDel.InvokeAsync(Item);
        }

        public async void OnStarClick()
        {
            if (OnStar.HasDelegate)
                await OnStar.InvokeAsync(Item);
        }
    }
}