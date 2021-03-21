using System;

namespace ToDo.Shared
{
    public class SetFinishReq
    {
        public Guid TaskId { get; set; }

        public bool IsFinish { get; set; }
    }
}