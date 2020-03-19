using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceCtrl.Data.Models.ClientSync
{
    public class ClientSyncDetails
    {
        public DateTime? LastSyncDate { get; set; }

        public List<SyncDetails>? SyncHistory { get; set; }

        public SyncDetails? SyncDetails { get; set; }
    }
}
