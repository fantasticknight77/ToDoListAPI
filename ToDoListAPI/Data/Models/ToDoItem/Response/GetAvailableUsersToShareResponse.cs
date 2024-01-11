namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Get available users to share response
    /// </summary>
    /// <seealso cref="GeneralResponse" />
    public class GetAvailableUsersToShareResponse : GeneralResponse
    {
        /// <summary>
        /// The available users to share.
        /// </summary>
        public List<AvailableUserToShare> Users { get; set; } = new List<AvailableUserToShare>();
    }
}
