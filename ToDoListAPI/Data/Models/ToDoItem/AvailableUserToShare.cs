namespace ToDoListAPI.Data.Models
{
    public class AvailableUserToShare
    {
        /// <summary>
        /// The identifier of user.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The username.
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }
}
