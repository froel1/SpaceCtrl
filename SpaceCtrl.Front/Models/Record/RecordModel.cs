using System;
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Data.Extensions;
using SpaceCtrl.Front.Models.Settings;
using Object = SpaceCtrl.Data.Database.DbObjects.Object;

namespace SpaceCtrl.Front.Models.Record
{
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

        public RecordModel(Person person, Object @object, ImageSettings settings)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            GroupName = person.Group.Name;
            CreateDate = @object.FrameDate;
            FramePath = @object.Frame.GetWebPath(settings, ImageType.Object);
        }
    }
}