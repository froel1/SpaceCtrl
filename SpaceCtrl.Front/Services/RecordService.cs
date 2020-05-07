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
using SpaceCtrl.Front.Models.Common;
using SpaceCtrl.Front.Models.Record;
using SpaceCtrl.Front.Models.Settings;
using Object = SpaceCtrl.Data.Database.DbObjects.Object;

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

        /*public List<Guid> Test(RecordFilterModel filter)
        {
            var date = (filter.Date ?? DateTime.Now).Date.AddDays(-1);

            var personKeys = (
                from obj in _dbContext.Object
                join per in _dbContext.Person on obj.PersonKey equals per.Key
                where obj.CreateDate >= date && obj.CreateDate <= date.AddDays(1)
                orderby obj.FrameDate
                group obj by obj.PersonKey
                into grp
                select grp.Key);

            var test = (from per in _dbContext.Person
                        join obj in _dbContext.Object on per.Key equals obj.PersonKey into objects
                        from finalObj in objects.Where(x => x.CreateDate >= date && x.CreateDate <= date).OrderBy(x => x.FrameDate).DefaultIfEmpty()
                        where personKeys.Contains(per.Key)
                        select new
                        {
                            person = per,
                            obj = finalObj
                        });

            return personKeys;
        }*/

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
                    _settings.Image.BasePath)
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

    [Flags]
    public enum WorkDays
    {
        None = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }

    public class PersonGroupModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public List<RecordModel> Records { get; set; }

        public PersonGroupModel(int? id, string name, RecordModel model)
        {
            Id = id;
            Name = name;
            Records = new List<RecordModel> { model };
        }

        public PersonGroupModel(PersonGroup groupModel)
        {
            Id = groupModel.Id;
            Name = groupModel.Name;
            Records = new List<RecordModel>();
        }
    }

    public class RecordModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GroupName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? FramePath { get; set; }

        public RecordModel(Person person, DateTime? date)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            GroupName = person.Group.Name;
            CreateDate = date;
        }

        public RecordModel(Person person, Object @object, string basePath)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            GroupName = person.Group.Name;
            CreateDate = @object.FrameDate;
            FramePath = @object.Frame.GetPath(basePath);
        }

        /*
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return (obj is RecordModel model) && Id == model.Id && CreateDate.AddMinutes(2) >= model.CreateDate;
        }

        public override int GetHashCode() => HashCode.Combine(Id, FirstName, LastName, CreateDate);
        */
    }
}