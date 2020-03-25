using System;
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
        public string Name { get; set; }
        public Guid Key { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Object> Object { get; set; }
    }
}
