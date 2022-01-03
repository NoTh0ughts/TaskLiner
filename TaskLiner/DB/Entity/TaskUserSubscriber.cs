using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class TaskUserSubscriber
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }

        [JsonIgnore]
        public virtual Task Task { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
