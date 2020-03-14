using System.ComponentModel.DataAnnotations;

namespace SpaceCtrl.Front.Models.Client
{
    public abstract class ClientModelBase
    {
        [Required]
        public string FirstName { get; set; } = default!;

        [Required]
        public string LastName { get; set; } = default!;
    }
}
