using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class TaskComment
    {
        public int Id { get; set; }
        public DateTime SpendedTime { get; set; }
        public string Description { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}
