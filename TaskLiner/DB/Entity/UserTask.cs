using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class UserTask
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public DateTime HoursWorked { get; set; }
        public bool IsOwner { get; set; }

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}
