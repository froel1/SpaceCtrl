using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SpaceCtrl.Data.Database.DbObjects;

namespace SpaceCtrl.Front.Extensions
{
    public static class HelperExt
    {
        public static string GetPath(this Frame frame, string basePath) =>
            Path.Combine(basePath, $"{frame.Id}{frame.Type}");
    }
}