using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class ShiftType
    {
        public ShiftType()
        {
            GroupShift = new HashSet<GroupShift>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<GroupShift> GroupShift { get; set; }
    }
}
