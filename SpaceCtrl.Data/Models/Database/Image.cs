using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Models.Database
{
    public partial class Image
    {
        public Image()
        {
            Object = new HashSet<Object>();
        }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Object> Object { get; set; }
    }
}
