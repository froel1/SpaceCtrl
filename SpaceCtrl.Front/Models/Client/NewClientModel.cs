using System.ComponentModel.DataAnnotations;

namespace SpaceCtrl.Front.Models.Client
{
    public class NewClientModel
    {
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; } = default!;

        [Required]
        public ClientType Type { get; set; }
    }

    public enum ClientType
    {
        Undefined = 0,
        Worker = 1
    }
}