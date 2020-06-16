using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class PersonImages
    {
        public int PersonId { get; set; }
        public Guid FrameId { get; set; }

        public virtual Frame Frame { get; set; }
        public virtual Person Person { get; set; }
    }
}
