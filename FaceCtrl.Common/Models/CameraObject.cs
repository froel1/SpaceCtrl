﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FaceCtrl.Common.Models
{
    public class CameraObject
    {
        public Guid Id { get; set; }

        public List<string> Images { get; set; }

        public CameraObject(Guid id, List<string> images)
        {
            Id = id;
            Images = images;
        }
    }
}
