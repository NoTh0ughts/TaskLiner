using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner
{
    public partial class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public string Scope { get; set; }
        public string Description { get; set; }
        public uint CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
