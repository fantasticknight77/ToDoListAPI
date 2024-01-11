namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Get users response
    /// </summary>
    /// <seealso cref="GeneralResponse" />
    public class GetUsersResponse : GeneralResponse
    {
        /// <summary>
        /// The users to be retrieved.
        /// </summary>
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
