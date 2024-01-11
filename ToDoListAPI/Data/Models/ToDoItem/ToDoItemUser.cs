namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// ToDo item user sharing relationship
    /// </summary>
    public class ToDoItemUser
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The identifier of todo item.
        /// </summary>
        public int ToDoItemID { get; set; }

        /// <summary>
        /// The identifier of user.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The todo item that this user is shared.
        /// </summary>
        public ToDoItem? ToDoItem { get; set; }

        /// <summary>
        /// The user that shared this todo item.
        /// </summary>
        public User? User { get; set; }
    }
}
