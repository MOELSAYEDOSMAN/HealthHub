using System.ComponentModel.DataAnnotations;

namespace HealthHup.API.Validation
{
    public class FileValidation: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var file=(IFormFile) value;
            if (file == null || file.Length == 0)
            {
                ErrorMessage = "No file uploaded.";
                return false;
            }
            // Validate the file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webg", ".bmp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                ErrorMessage = "Invalid file type. Only images are allowed.";
                return false;
            }
            // Optionally, validate the file content type
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/bmp" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                ErrorMessage = "Invalid MIME type. Only images are allowed.";
                return false;
            }
            // Validate the file size (limit to 2MB)
            var maxFileSize = 2 * 1024 * 1024; // 2MB
            if (file.Length > maxFileSize)
            {
                ErrorMessage = "File size exceeds the limit of 2MB.";
                return false;
            }
            return true;
        }
    }
}
