using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceCtrl.Data.Database.DbObjects;

namespace SpaceCtrl.Front.Models.Group
{
    public class GroupScheduleDetails
    {
        public List<GroupScheduleDetail> Details { get; set; } = default!;
    }

    public class GroupScheduleDetail
    {
        public int WeekNumber { get; set; }

        public ShiftType ShiftType { get; set; } = default!;
    }
}