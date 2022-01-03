using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class TaskSubscriber
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}
