using System.ComponentModel.DataAnnotations;
using SpaceCtrl.Front.Services;

namespace SpaceCtrl.Front.Models.Client.Groups
{
    public class GroupModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public WorkDays WorkDays { get; set; }
    }
}