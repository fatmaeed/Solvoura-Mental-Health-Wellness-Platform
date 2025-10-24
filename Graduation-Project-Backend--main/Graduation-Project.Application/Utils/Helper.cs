using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Utils
{
    public class Helper
    {
        public static async Task<string> SaveIconAsync(IFormFile icon, string prefix)
        {
            if (icon == null || icon.Length == 0)
            {
                throw new ArgumentException("Icon file is required.");
            }
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            var extension = Path.GetExtension(icon.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Invalid icon file type. Allowed types are: " + string.Join(", ", allowedExtensions));
            }
            var safeFileName = $"{DateTime.Now:yyyyMMdd_HHmmssfff}{extension}";
            var filePath = Path.Combine($"wwwroot/Images/{prefix}", safeFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await icon.CopyToAsync(stream);
            }
            return $"/Images/{prefix}/{safeFileName}";
        }

    }
}
