using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner
{
    public partial class TaskUserSubscriber
    {
        public uint UserId { get; set; }
        public uint TaskId { get; set; }

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}
