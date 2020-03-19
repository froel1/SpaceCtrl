using System.ComponentModel.DataAnnotations;

namespace SpaceCtrl.Front.Models.Client
{
    public class NewClientModel 
    {
        [Required]
        public string FirstName { get; set; } = default!;

        [Required]
        public string LastName { get; set; } = default!;

        public ClientType Type { get; set; }
    }

    public enum ClientType
    {
        Undefined = 0,
        Worker = 1
    }
}