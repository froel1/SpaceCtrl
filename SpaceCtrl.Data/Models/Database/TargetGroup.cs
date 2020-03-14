using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class TargetGroup
    {
        public TargetGroup()
        {
            Client = new HashSet<Client>();
            Device = new HashSet<Device>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string EncodingsPath { get; set; }

        public virtual ICollection<Client> Client { get; set; }
        public virtual ICollection<Device> Device { get; set; }
    }
}
