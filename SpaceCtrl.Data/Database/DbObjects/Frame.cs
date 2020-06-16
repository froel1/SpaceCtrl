using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class Frame
    {
        public Frame()
        {
            GroupEntry = new HashSet<GroupEntry>();
            Object = new HashSet<Object>();
            PersonImages = new HashSet<PersonImages>();
        }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<GroupEntry> GroupEntry { get; set; }
        public virtual ICollection<Object> Object { get; set; }
        public virtual ICollection<PersonImages> PersonImages { get; set; }
    }
}
