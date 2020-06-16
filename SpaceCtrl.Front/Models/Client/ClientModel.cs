using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceCtrl.Front.Models.Client
{
    public class ClientModel
    {
        public int? Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; } = default!;

        public List<ImageListModel>? Images { get; set; }

        public int GroupId { get; set; }
    }

    public class ImageListModel
    {
        public Guid Id { get; set; }

        public string Path { get; set; }
    }
}