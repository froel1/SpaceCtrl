using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class Person
    {
        public int Id { get; set; }
        public Guid Key { get; set; }
        public DateTime CreateDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? SyncRequestedAt { get; set; }
        public string SyncDetails { get; set; }
    }
}
