using System.ComponentModel.DataAnnotations;

namespace Memo.App.Api.Models
{
    public class UserDto : IValidatableObject
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Family { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.Equals("admin", StringComparison.OrdinalIgnoreCase) || UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("استفاده از این نام کاربری مجاز نیست.", new[] { nameof(UserName) });
        }
    }
}
