﻿using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner
{
    public partial class Task
    {
        public uint Id { get; set; }
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
        public uint ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
