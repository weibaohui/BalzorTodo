using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign.Charts;
using Microsoft.AspNetCore.Components;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class Statistics
    {
        private readonly StackedColumnConfig amountConfig = new()
        {
            Title = new Title
            {
                Visible = true,
                Text = "每日代办数量统计"
            },
            ForceFit = true,
            Padding = "auto",
            XField = "day",
            YField = "value",
            YAxis = new ValueAxis
            {
                Min = 0
            },
            Meta = new
            {
                day = new
                {
                    Alias = "日期"
                }
            },
            Color = new[] {"#ae331b", "#1a6179"},
            StackField = "type"
        };

        private IChartComponent amountChart;

        private bool isLoading;
        [Inject] public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            var amountData = await Http.GetFromJsonAsync<List<ChartAmountDto>>("api/Chart/GetAmountDto");
            await amountChart.ChangeData(amountData);
            await base.OnInitializedAsync();

            isLoading = false;
        }
    }
}