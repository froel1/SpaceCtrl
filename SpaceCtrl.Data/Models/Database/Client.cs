using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class Client
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreateDate { get; set; }
        public int TargetId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }

        public virtual TargetGroup Target { get; set; }
    }
}
