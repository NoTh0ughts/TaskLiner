using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.entity
{
    public partial class Company
    {
        public Company()
        {
            Projects = new HashSet<Project>();
            WorkerContracts = new HashSet<WorkerContract>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<WorkerContract> WorkerContracts { get; set; }
    }
}
