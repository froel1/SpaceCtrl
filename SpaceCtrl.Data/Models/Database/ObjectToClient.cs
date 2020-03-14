using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class ObjectToClient
    {
        public int ObjectId { get; set; }
        public int ClientId { get; set; }

        public virtual Object Object { get; set; }
    }
}
