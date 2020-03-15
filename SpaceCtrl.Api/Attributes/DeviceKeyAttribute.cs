using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SpaceCtrl.Api.Services;

namespace SpaceCtrl.Api.Attributes
{
    public class DeviceKeyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (!(value is Guid id) || id == Guid.Empty)
                return new ValidationResult(HttpStatusCode.Forbidden.ToString("G"));

            if (!DeviceExist(id, validationContext))
                return new ValidationResult(HttpStatusCode.Forbidden.ToString("G"));

            return ValidationResult.Success;
        }

        private static bool DeviceExist(Guid deviceKey, IServiceProvider validationContext) =>
            (validationContext.GetService(typeof(DeviceService)) as DeviceService)?.ExistAsync(deviceKey).GetAwaiter().GetResult() ?? false;

    }
}
