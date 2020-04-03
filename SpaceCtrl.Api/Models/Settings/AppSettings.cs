﻿namespace SpaceCtrl.Api.Models.Settings
{
    public class AppSettings
    {
        public SwaggerOptions Swagger { get; set; } = default!;

        public string ImagePath { get; set; } = default!;
    }
}