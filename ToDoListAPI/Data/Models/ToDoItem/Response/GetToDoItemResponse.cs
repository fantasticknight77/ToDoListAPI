namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Get todo item response
    /// </summary>
    /// <seealso cref="GeneralResponse" />
    public class GetToDoItemResponse : GeneralResponse
    {
        /// <summary>
        /// The todo item to be retrieved.
        /// </summary>
        public ToDoItemDTO? ToDoItem { get; set; }
    }
}
