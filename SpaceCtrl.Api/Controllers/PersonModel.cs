using System;
using System.Collections.Generic;

namespace SpaceCtrl.Api.Controllers
{
    public class PersonModel
    {
        public List<Guid> Ids { get; set; } = default!;

        public Direction Direction { get; set; }

        public string Image { get; set; }
    }
}