using System.Collections.Generic;
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Data.Helpers;
using SpaceCtrl.Front.Models.Group;

namespace SpaceCtrl.Front.Extensions
{
    public static class GroupExt
    {
        public static List<GroupScheduleDetail> GetGroupDetails(this PersonGroup group)
        {
            return group.Details.DeserializeToObject<GroupScheduleDetails>()!.Details;
        }
    }
}