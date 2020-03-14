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

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Object> Object { get; set; }
    }
}
