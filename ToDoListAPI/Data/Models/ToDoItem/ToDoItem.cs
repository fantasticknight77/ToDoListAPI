using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// ToDo item status constans
    /// </summary>
    public class ToDoItemStatus
    {
        public const string NOTSTARTED = "Not Started";
        public const string INPROGRESS = "In Progress";
        public const string COMPLETED = "Completed";
    }

    /// <summary>
    /// ToDo item priority constants
    /// </summary>
    public class ToDoItemPriority
    {
        public const string LOW = "Low";
        public const string MEDIUM = "Medium";
        public const string HIGH = "High";
    }

    /// <summary>
    /// ToDo item
    /// </summary>
    public class ToDoItem
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
        public User? User { get; set; }

        /// <summary>
        /// the tags fo todo item.
        /// </summary>
        public virtual ICollection<ToDoItemTag>? ToDoItemTags { get; set; }

        /// <summary>
        /// The concurrency token that is used to implement optimistic concurrency control.
        /// </summary>
        [Timestamp]
        public byte[] ConcurrencyToken { get; set; } = new byte[0];
    }
}
