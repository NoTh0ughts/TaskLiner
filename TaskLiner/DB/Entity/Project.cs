using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class Project
    {
        public Project()
        {
            ProjectAccesses = new HashSet<ProjectAccess>();
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Scope { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<ProjectAccess> ProjectAccesses { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
