using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner
{
    public partial class WorkerContract
    {
        public uint UserId { get; set; }
        public uint CompanyId { get; set; }
        public bool IsOwner { get; set; }

        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
    }
}
