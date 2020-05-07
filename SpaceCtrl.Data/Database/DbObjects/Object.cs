using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class Object
    {
        public int Id { get; set; }
        public Guid PersonKey { get; set; }
        public Guid FrameId { get; set; }
        public int DeviceId { get; set; }
        public int ChannelId { get; set; }
        public int Direction { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FrameDate { get; set; }
        public int PersonCount { get; set; }

        public virtual Device Device { get; set; }
        public virtual Frame Frame { get; set; }
    }
}
