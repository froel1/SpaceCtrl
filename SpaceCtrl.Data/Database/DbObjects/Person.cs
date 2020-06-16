using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class Person
    {
        public Person()
        {
            GroupEntry = new HashSet<GroupEntry>();
            PersonImages = new HashSet<PersonImages>();
        }

        public int Id { get; set; }
        public Guid Key { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? SyncRequestedAt { get; set; }
        public string SyncDetails { get; set; }
        public int GroupId { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual PersonGroup Group { get; set; }
        public virtual ICollection<GroupEntry> GroupEntry { get; set; }
        public virtual ICollection<PersonImages> PersonImages { get; set; }
    }
}
