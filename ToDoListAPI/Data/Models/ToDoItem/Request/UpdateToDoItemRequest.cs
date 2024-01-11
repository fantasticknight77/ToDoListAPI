using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Data.Models
{
    public class UpdateToDoItemRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "DueDate is required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = ToDoItemStatus.NOTSTARTED;

        [Required(ErrorMessage = "Priority is required")]
        public string Priority { get; set; } = ToDoItemPriority.LOW;

        public List<string> ToDoItemTags { get; set; } = new List<string>();

        /// <summary>The concurrency token that is used to implement optimistic concurrency control.</summary>
        [Timestamp]
        [Required(ErrorMessage = "Concurrency token is required")]
        public byte[] ConcurrencyToken { get; set; } = new byte[0];
    }
}
