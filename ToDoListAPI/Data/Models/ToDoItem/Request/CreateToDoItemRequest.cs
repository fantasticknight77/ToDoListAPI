using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Create todo item request
    /// </summary>
    public class CreateToDoItemRequest
    {
        /// <summary>
        /// Name of the todo item.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the todo item.
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Due Date of the todo item.
        /// </summary>
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "DueDate is required")]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Priority of the todo item.
        /// </summary>
        [Required(ErrorMessage = "Priority is required")]
        public string Priority { get; set; } = ToDoItemPriority.LOW;
    }
}
