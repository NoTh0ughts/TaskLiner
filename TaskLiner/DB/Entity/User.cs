using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.entity
{
    public partial class User
    {
        public User()
        {
            WorkerContracts = new HashSet<WorkerContract>();
        }

        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public string Proffesion { get; set; }
        public string Password { get; set; }

        public virtual ICollection<WorkerContract> WorkerContracts { get; set; }
    }
}
