using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class Object
    {
        public int Id { get; set; }
        public Guid PersonKey { get; set; }
        public DateTime ImageDate { get; set; }
        public Guid ImageId { get; set; }
        public int DeviceId { get; set; }
        public int Direction { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Device Device { get; set; }
        public virtual Image Image { get; set; }
    }
}
