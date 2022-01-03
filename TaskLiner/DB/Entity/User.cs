using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class User
    {
        public User()
        {
            TaskComments = new HashSet<TaskComment>();
            TaskSubscribers = new HashSet<TaskSubscriber>();
            UserTasks = new HashSet<UserTask>();
            WorkerContracts = new HashSet<WorkerContract>();
        }

        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Proffesion { get; set; }
        public string Password { get; set; }

        public virtual ICollection<TaskComment> TaskComments { get; set; }
        public virtual ICollection<TaskSubscriber> TaskSubscribers { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }
        public virtual ICollection<WorkerContract> WorkerContracts { get; set; }
    }
}
