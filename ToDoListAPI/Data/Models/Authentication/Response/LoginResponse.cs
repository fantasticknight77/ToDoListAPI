namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Login response
    /// </summary>
    /// <seealso cref="GeneralResponse" />
    public class LoginResponse : GeneralResponse
    {
        /// <summary>
        /// JWT token to used by user for authorization.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
