namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Response status constants
    /// </summary>
    public class ResponseStatus
    {
        public const string SUCCESS = "Success";
        public const string ERROR = "Error";
    }

    /// <summary>
    /// General response
    /// </summary>
    public class GeneralResponse
    {
        /// <summary>
        /// Status of the response.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Message of the response.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
