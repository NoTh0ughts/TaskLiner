using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner
{
    public partial class User
    {
        public User()
        {
            WorkerContracts = new HashSet<WorkerContract>();
        }

        public uint Id { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
        public string Proffesion { get; set; }

        public virtual ICollection<WorkerContract> WorkerContracts { get; set; }
    }
}
