﻿using System;
using System.Collections.Generic;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class WorkerContract
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public bool IsOwner { get; set; }

        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
        public virtual ProjectAccess ProjectAccess { get; set; }
    }
}
