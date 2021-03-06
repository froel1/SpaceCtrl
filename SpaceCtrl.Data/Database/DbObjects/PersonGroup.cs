﻿using System;
using System.Collections.Generic;

namespace SpaceCtrl.Data.Database.DbObjects
{
    public partial class PersonGroup
    {
        public PersonGroup()
        {
            GroupShift = new HashSet<GroupShift>();
            Person = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }

        public virtual ICollection<GroupShift> GroupShift { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}
