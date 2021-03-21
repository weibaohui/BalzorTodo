using System.Collections.Generic;

namespace ToDo.Shared
{
    public class GetSearchRsp
    {
        public List<TaskDto> Data { get; set; }

        public int Total { get; set; }
    }
}