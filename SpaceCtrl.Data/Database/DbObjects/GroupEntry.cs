using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class GroupEntry
    {
        public int Id { get; set; }
        public int GroupShiftId { get; set; }
        public int? PersonId { get; set; }
        public Guid? FrameId { get; set; }
        public int Direction { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Frame Frame { get; set; }
        public virtual GroupShift GroupShift { get; set; }
        public virtual Person Person { get; set; }
    }
}
