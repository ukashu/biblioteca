using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace biblioteca.Helpers
{
    public static class ImagePathHelper
    {
        public static string GetCoverPath(string fileName)
        {
            var baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "biblioteca", "Covers");

            var coverPath = Path.Combine(baseDir, $"{fileName}.png");
            var fallbackPath = Path.Combine(baseDir, "default.png");

            return File.Exists(coverPath) ? coverPath : fallbackPath;
        }
    }
}
