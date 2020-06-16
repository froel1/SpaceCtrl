using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceCtrl.Front.Models.Client
{
    public class ClientDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public ClientDetails(int id, string firstName, string lastName, string groupName, DateTime date, bool isActive)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            GroupName = groupName;
            CreateDate = date;
            IsActive = isActive;
        }
    }
}