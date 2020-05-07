using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class Device
    {
        public Device()
        {
            Channel = new HashSet<Channel>();
            Object = new HashSet<Object>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Guid Key { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Channel> Channel { get; set; }
        public virtual ICollection<Object> Object { get; set; }
    }
}
