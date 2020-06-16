using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Front.Extensions;
using SpaceCtrl.Front.Models.Group;

namespace SpaceCtrl.Front.Services
{
    public class GroupService
    {
        private readonly SpaceCtrlContext _dbContext;

        public GroupService(SpaceCtrlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateGroupScheduleAsync()
        {
            var groups = await _dbContext.PersonGroup.Include(x => x.GroupShift).ToListAsync();
            var shifts = new List<GroupShift>();

            foreach (var personGroup in groups)
            {
                var shift = personGroup.GroupShift.OrderByDescending(x => x.Id).FirstOrDefault();
                if (shift is null)
                {
                    var group = personGroup.GetGroupDetails().First(x => x.WeekNumber == 1);
                    AddFirstWeek(personGroup, shifts, group);
                    continue;
                }
                else
                {
                    var groupDetails = personGroup.GetGroupDetails();
                    var shiftIndex = shift.WeekNumber % groupDetails.Count;
                    var group = groupDetails.FirstOrDefault(x => x.WeekNumber == (shiftIndex == 0 ? 1 : shiftIndex));
                }

                if (shift.EndDate < DateTime.Now)
                    continue;
            }

            await _dbContext.GroupShift.AddRangeAsync(shifts);
        }

        private static void AddFirstWeek(PersonGroup personGroup, List<GroupShift> shifts, GroupScheduleDetail group)
        {
            var startDate = DateTime.Now;

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                startDate = DateTime.Now.Date + group.ShiftType.StartDate.TimeOfDay;

            var endDate = startDate.AddDays(7 - (int)startDate.DayOfWeek).AddDays(1).Date;

            shifts.Add(new GroupShift
            {
                WeekNumber = 1,
                StartDate = startDate,
                EndDate = endDate,
                GroupId = personGroup.Id,
                ShiftType = group.ShiftType.Id
            });
        }
    }
}