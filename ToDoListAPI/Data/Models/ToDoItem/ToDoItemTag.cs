using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// ToDo Item Tag
    /// </summary>
    public class ToDoItemTag
    {
        /// <summary>
        /// The identifier ot todo item tag.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// The identifier ot todo item.
        /// </summary>
        [Required(ErrorMessage = "ToDoItemID is required")]
        public int ToDoItemID { get; set; }

        /// <summary>
        /// The todo item that this todo item tag belongs to.
        /// </summary>
        public ToDoItem? ToDoItem { get; set; }

        /// <summary>
        /// The name ot todo item tag.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The concurrency token that is used to implement optimistic concurrency control.
        /// </summary>
        [Timestamp]
        public byte[] ConcurrencyToken { get; set; } = new byte[0];
    }
}
