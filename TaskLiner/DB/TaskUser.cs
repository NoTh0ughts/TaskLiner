using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner
{
    public partial class TaskUser
    {
        public uint TaskId { get; set; }
        public uint UserId { get; set; }
        public DateTime HoursWorked { get; set; }
        public bool IsOwner { get; set; }

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}
