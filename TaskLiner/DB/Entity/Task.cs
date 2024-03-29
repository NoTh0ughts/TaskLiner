﻿using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class Task
    {
        public Task()
        {
            TaskComments = new HashSet<TaskComment>();
            TaskSubscribers = new HashSet<TaskSubscriber>();
            UserTasks = new HashSet<UserTask>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Checklist { get; set; }
        public string Content { get; set; }
        public string State { get; set; }
        public string ColumnState { get; set; }
        public DateTime SpendedTime { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; }
        public virtual ICollection<TaskSubscriber> TaskSubscribers { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
