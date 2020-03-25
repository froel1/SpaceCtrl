using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceCtrl.Data.Models.ClientSync
{
    public class PersonSyncDetails
    {
        public DateTime? LastSyncDate { get; set; }

        public List<SyncDetails>? SyncHistory { get; set; }

        public SyncDetails? SyncDetails { get; set; }
    }
}
