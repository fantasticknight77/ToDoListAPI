using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Register request
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Username of the user.
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email of the user.
        /// </summary>
        [EmailAddress(ErrorMessage = "You have entered an invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password of the user.
        /// </summary>
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
