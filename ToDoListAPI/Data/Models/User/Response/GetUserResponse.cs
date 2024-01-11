namespace ToDoListAPI.Data.Models
{
    /// <summary>
    ///  Get user response
    /// </summary>
    /// <seealso cref="GeneralResponse" />
    public class GetUserResponse : GeneralResponse
    {
        /// <summary>
        /// The user to be retrieved.
        /// </summary>
        public UserDTO? User { get; set; }
    }
}
