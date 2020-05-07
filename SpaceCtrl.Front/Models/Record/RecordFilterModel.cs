using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceCtrl.Front.Models.Record
{
    public class RecordFilterModel
    {
        public DateTime? Date { get; set; }

        public string? Name { get; set; }

        public int? GroupId { get; set; }
    }
}