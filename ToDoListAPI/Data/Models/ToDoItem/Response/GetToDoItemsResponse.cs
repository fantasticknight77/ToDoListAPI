namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Get todo items response
    /// </summary>
    /// <seealso cref="GeneralResponse" />
    public class GetToDoItemsResponse : GeneralResponse
    {
        /// <summary>
        /// Todo items to be retrieved.
        /// </summary>
        public IEnumerable<ToDoItemDTO> ToDoItems { get; set; } = Enumerable.Empty<ToDoItemDTO>();
    }
}
