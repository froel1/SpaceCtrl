﻿using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class Device
    {
        public Device()
        {
            Object = new HashSet<Object>();
        }

        public int Id { get; set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public int? OrderIndex { get; set; }
        public int TargetId { get; set; }

        public virtual TargetGroup Target { get; set; }
        public virtual ICollection<Object> Object { get; set; }
    }
}
