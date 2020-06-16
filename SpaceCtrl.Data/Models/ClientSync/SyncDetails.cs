using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.ClientSync
{
    public class SyncDetails
    {
        public SyncOperationType Type { get; set; }

        public DateTime? SyncDate { get; set; }
    }
}