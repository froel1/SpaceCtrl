using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class GroupShift
    {
        public GroupShift()
        {
            GroupEntry = new HashSet<GroupEntry>();
        }

        public int Id { get; set; }
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GroupId { get; set; }
        public int ShiftType { get; set; }

        public virtual PersonGroup Group { get; set; }
        public virtual ShiftType ShiftTypeNavigation { get; set; }
        public virtual ICollection<GroupEntry> GroupEntry { get; set; }
    }
}
