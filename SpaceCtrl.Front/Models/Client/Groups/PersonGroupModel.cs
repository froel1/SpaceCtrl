using System.Collections.Generic;
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Front.Models.Record;
using SpaceCtrl.Front.Services;

namespace SpaceCtrl.Front.Models.Client.Groups
{
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
}