using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Front.Extensions;
using SpaceCtrl.Front.Models.Client.Groups;
using SpaceCtrl.Front.Models.Common;
using SpaceCtrl.Front.Models.Record;
using SpaceCtrl.Front.Models.Settings;

namespace SpaceCtrl.Front.Services
{
    public class RecordService
    {
        private readonly SpaceCtrlContext _dbContext;
        private readonly AppSettings _settings;

        public RecordService(SpaceCtrlContext dbContext, IOptions<AppSettings> options)
        {
            _dbContext = dbContext;
            _settings = options.Value;
        }

        public async Task<PagedList<RecordModel>> GetRecordsAsync(PaginationWithFilter<RecordFilterModel> filter)
        {
            var filterParams = new
            {
                groupId = filter.Filter?.GroupId,
                date = (filter.Filter?.Date ?? DateTime.Now).Date,
                name = string.IsNullOrEmpty(filter.Filter?.Name) ? null : filter.Filter.Name
            };

            var rawData = await (
                from obj in _dbContext.Object
                join per in _dbContext.Person on obj.PersonKey equals per.Key
                join grp in _dbContext.PersonGroup on per.GroupId equals grp.Id
                where
                    (obj.FrameDate >= filterParams.date && obj.FrameDate <= filterParams.date.AddDays(1)) &&
                    (!filterParams.groupId.HasValue || per.GroupId == filterParams.groupId) &&
                    (filterParams.name == null || (per.FirstName + per.LastName).Contains(filterParams.name))
                select new
                {
                    Obj = obj,
                    Frame = obj.Frame,
                    Grp = grp,
                    Per = per
                }).ToListAsync();

            var count = rawData.GroupBy(x => x.Obj.PersonKey).Count();

            var data = rawData.GroupBy(x => x.Obj.PersonKey).Select(x =>
                new RecordModel(
                    x.First().Per,
                    x.OrderBy(t => t.Obj.FrameDate).First().Obj,
                    _settings.Image)
            ).OrderBy(x => x.CreateDate).ToPagedList(filter).ToList();

            return new PagedList<RecordModel>(count, data);
        }

        public async Task<List<PersonGroupModel>> GetAttendancesAsync(RecordFilterModel filter)
        {
            var date = (filter.Date ?? DateTime.Now).Date;

            var rawData = await (
                from grp in _dbContext.PersonGroup
                join per in _dbContext.Person on grp.Id equals per.GroupId
                join obj in _dbContext.Object on per.Key equals obj.PersonKey into persons
                from perObj in persons.DefaultIfEmpty()
                where (perObj == null || (perObj.CreateDate >= date && perObj.CreateDate <= date.AddDays(1)))
                select new
                {
                    Group = grp,
                    Person = per,
                    Record = perObj
                }).ToListAsync();

            var result = new List<PersonGroupModel>();

            foreach (var data in rawData)
            {
                var group = result.FirstOrDefault(x => x.Id == data.Group.Id);
                if (group is null)
                {
                    group = new PersonGroupModel(data.Group);
                    result.Add(group);
                }

                var person = group.Records.FirstOrDefault(x => x.Id == data.Person.Id);
                if (person is null)
                    group.Records.Add(new RecordModel(data.Person, data.Record?.CreateDate));
                else if (data.Record?.CreateDate != null && person.CreateDate < data.Record.CreateDate)
                    person.CreateDate = data.Record.CreateDate;
            }

            return result;
        }
    }
}