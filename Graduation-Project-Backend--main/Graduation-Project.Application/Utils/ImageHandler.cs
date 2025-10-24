using Microsoft.AspNetCore.Http;

namespace Graduation_Project.Application.Utils {

    public class UploadingImageStatus {

        protected UploadingImageStatus(string message) {
            Message = message;
        }

        public string Message { get; }
    }

    public class UploadImageSuccess : UploadingImageStatus {
        public string Path { get; }

        public UploadImageSuccess(string path, string message = "Image uploaded successfully") : base(message) {
            Path = path;
        }
    }

    public class UploadImageFailed : UploadingImageStatus {

        public UploadImageFailed(string message) : base(message) {
        }
    }

    public class ImageHandler {
        private static readonly string[] _allowedEextensions = new[] { ".jpg", ".png", ".jpeg" };

        public static async Task<UploadingImageStatus> UploadImage(IFormFile image, string prefix) {
            try {
                var allowedSize = 5 * 1024 * 1024;

                if (image.Length > allowedSize) {
                    return new UploadImageFailed("Image size must be less than 5MB");
                }

                var extension = Path.GetExtension(image.FileName).ToLower();

                if (!_allowedEextensions.Contains(extension)) {
                    return new UploadImageFailed("Extension is not allowed");
                }

                var safeFileName = $"{DateTime.Now:yyyyMMdd_HHmmssfff}{extension}";

                var filePath = Path.Combine($"wwwroot/Images/{prefix}", safeFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    await image.CopyToAsync(stream);
                }

                return new UploadImageSuccess($"/Images/{prefix}/{safeFileName}");
            } catch (Exception) {
                return new UploadImageFailed("Image upload failed");
            }
        }
    }
}