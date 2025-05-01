using System.ComponentModel.DataAnnotations;

namespace Webbankhoahoconline.Repositories.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { ".jpg", ".png", ".jpeg" };

                bool result = extensions.Any(x => extension.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult($"File không hợp lệ, chỉ chấp nhận các định dạng: {string.Join(", ", extensions)}");
                }
            }
                return ValidationResult.Success;
        }
    }
    
   }
