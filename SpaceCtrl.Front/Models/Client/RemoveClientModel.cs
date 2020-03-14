using System.ComponentModel.DataAnnotations;

namespace SpaceCtrl.Front.Models.Client
{
    public class RemoveClientModel
    {
        [Required]
        public int Id { get; set; }
    }
}