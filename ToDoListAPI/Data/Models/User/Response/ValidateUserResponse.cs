namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Validate user response
    /// </summary>
    /// <seealso cref="GeneralResponse" />
    public class ValidateUserResponse : GeneralResponse
    {
        /// <summary>
        /// The user returned after validation succeed.
        /// </summary>
        public User? User { get; set; }
    }
}
