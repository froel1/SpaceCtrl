using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.ClientSync
{
    public class SyncDetails
    {
        public string? ImagePath { get; set; }

        public List<string>? Images { get; set; }

        public SyncOperationType Type { get; set; }

        public DateTime? SyncDate { get; set; }
    }
}