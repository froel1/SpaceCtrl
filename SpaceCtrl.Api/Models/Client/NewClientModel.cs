using System.Security.Cryptography;

namespace SpaceCtrl.Api.Models.Client
{
    public class NewClientModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}