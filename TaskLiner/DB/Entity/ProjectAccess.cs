using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class ProjectAccess
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int ProjectId { get; set; }

        public virtual WorkerContract Contract { get; set; }
        public virtual Project Project { get; set; }
    }
}
