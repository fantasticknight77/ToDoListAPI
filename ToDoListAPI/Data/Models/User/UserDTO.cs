using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Data.Models
{
    public class UserDTO
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
        /// the email of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// the role of the user.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// The concurrency token that is used to implement optimistic concurrency control.
        /// </summary>
        [Timestamp]
        public byte[] ConcurrencyToken { get; set; } = new byte[0];
    }
}
