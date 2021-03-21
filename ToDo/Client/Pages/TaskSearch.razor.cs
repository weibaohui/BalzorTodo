using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class TaskSearch
    {
        private List<TaskDto> datas = new();

        private bool isLoading;

        private string queryTitle;

        private int total;

        //7、	查询待办
        [Inject] public HttpClient Http { get; set; }

        //8、	查看详细服务
        [Inject] public TaskDetailServices TaskSrv { get; set; }

        private async Task OnSearch()
        {
            await OnQuery(1, 10, new List<SortFieldName>());
        }

        private async Task OnChange(QueryModel<TaskDto> queryModel)
        {
            await OnQuery(
                queryModel.PageIndex,
                queryModel.PageSize,
                queryModel.SortModel.Where(x => string.IsNullOrEmpty(x.Sort) == false).OrderBy(x => x.Priority)
                    .Select(x => new SortFieldName {SortField = x.FieldName, SortOrder = x.Sort}).ToList()
            );
        }

        private async Task OnQuery(int pageIndex, int pageSize, List<SortFieldName> sort)
        {
            isLoading = true;
            var req = new GetSearchReq
            {
                QueryTitle = queryTitle,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Sorts = sort
            };
            var httpRsp = await Http.PostAsJsonAsync("api/Task/GetSearch", req);
            var result = await httpRsp.Content.ReadFromJsonAsync<GetSearchRsp>();
            datas = result.Data;
            total = result.Total;

            isLoading = false;
        }

        private async Task OnDetail(TaskDto taskDto)
        {
            await TaskSrv.EditTask(taskDto, datas);
        }
    }
}