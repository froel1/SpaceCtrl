using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class Object
    {
        public Object()
        {
            ObjectToClient = new HashSet<ObjectToClient>();
        }

        public int Id { get; set; }
        public int Direction { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid DeviceKey { get; set; }
        public int ImageId { get; set; }

        public virtual Device DeviceKeyNavigation { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<ObjectToClient> ObjectToClient { get; set; }
    }
}
