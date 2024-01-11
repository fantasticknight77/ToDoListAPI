using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// User roles constants
    /// </summary>
    public class UserRoles
    {
        public const string ADMIN = "Admin";
        public const string USER = "User";
    }

    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>
        /// the identifier of the user.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// the username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// the password hash of the user.
        /// </summary>
        public byte[] PasswordHash { get; set; } = new byte[32];

        /// <summary>
        /// the password salt of the user.
        /// </summary>
        public byte[] PasswordSalt { get; set; } = new byte[32];

        /// <summary>
        /// the email of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// the role of the user.
        /// </summary>
        public string Role {  get; set; } = string.Empty;

        /// <summary>
        /// The concurrency token that is used to implement optimistic concurrency control.
        /// </summary>
        [Timestamp]
        public byte[] ConcurrencyToken { get; set; } = new byte[0];
    }
}
