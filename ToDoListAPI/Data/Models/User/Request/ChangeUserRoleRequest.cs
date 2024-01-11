using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Change user role request
    /// </summary>
    public class ChangeUserRoleRequest
    {
        /// <summary>
        /// The role of the user.
        /// </summary>
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// The concurrency token of the user.
        /// </summary>
        [Required(ErrorMessage = "Concurrency token is required")]
        public byte[] ConcurrencyToken { get; set; } = new byte[0];
    }
}
