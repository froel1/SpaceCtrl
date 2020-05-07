using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class Frame
    {
        public Frame()
        {
            Object = new HashSet<Object>();
        }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Object> Object { get; set; }
    }
}
