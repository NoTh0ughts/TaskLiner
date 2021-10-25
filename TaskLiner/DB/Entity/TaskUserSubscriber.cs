using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.entity
{
    public partial class TaskUserSubscriber
    {
        public int UserId { get; set; }
        public int TaskId { get; set; }

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}
