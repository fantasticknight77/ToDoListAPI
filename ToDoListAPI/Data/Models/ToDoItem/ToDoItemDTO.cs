using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// ToDo item dto
    /// </summary>
    public class ToDoItemDTO
    {
        /// <summary>
        /// the identifier fo todo item.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// the name fo todo item.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// the description fo todo item.
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// the due date fo todo item.
        /// </summary>
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "DueDate is required")]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// the status fo todo item.
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = ToDoItemStatus.NOTSTARTED;

        /// <summary>
        /// the priority fo todo item.
        /// </summary>
        [Required(ErrorMessage = "Priority is required")]
        public string Priority { get; set; } = ToDoItemPriority.LOW;

        /// <summary>
        /// the owner id fo todo item.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// the owner fo todo item.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// the tags fo todo item.
        /// </summary>
        public List<string> ToDoItemTagNames { get; set; } = new List<string>();

        /// <summary>
        /// The concurrency token that is used to implement optimistic concurrency control.
        /// </summary>
        [Timestamp]
        public byte[] ConcurrencyToken { get; set; } = new byte[0];
    }
}
