using System;
using System.ComponentModel.DataAnnotations;

namespace SpaceCtrl.Api.Models
{
    public class DeviceModel
    {
        [Required]
        public Guid Key { get; set; }

        public int DeviceId { get; set; }

        public string? Name { get; set; }
        public int? OrderIndex { get; set; }

        [Required]
        public int TargetId { get; set; }
    }
}