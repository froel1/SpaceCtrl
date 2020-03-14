using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class ObjectToClient
    {
        public int ObjectId { get; set; }
        public Guid ObjectGuid { get; set; }

        public virtual Object Object { get; set; }
    }
}
