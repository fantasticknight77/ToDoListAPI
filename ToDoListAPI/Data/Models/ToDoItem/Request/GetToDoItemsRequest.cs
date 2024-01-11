namespace ToDoListAPI.Data.Models
{
    /// <summary>
    /// Sort order constants
    /// </summary>
    public class SortOrder
    {
        public const string ASC = "ASC";
        public const string DESC = "DESC";
    }

    /// <summary>
    /// Filter options for todo item list
    /// </summary>
    public class FilterOptions
    {
        /// <summary>
        /// The filter conditions.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The status filter conditions.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// The priority filter conditions.
        /// </summary>
        public string? Priority { get; set; }

        /// <summary>
        /// The tag filter conditions.
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// Start date of to filter todo item based on due date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// End date of to filter todo item based on due date.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// Sort options for todo item list
    /// </summary>
    public class SortOptions
    {
        /// <summary>
        /// The sort condition name (including Name, Status, Priority, DueDate).
        /// </summary>
        public string SortConditionName { get; set; } = string.Empty;

        /// <summary>
        /// The sort condition value.
        /// </summary>
        public string SortConditionValue { get; set; } = SortOrder.ASC;
    }

    /// <summary>
    /// Get todo item request
    /// </summary>
    public class GetToDoItemsRequest
    {
        public FilterOptions? FilterOptions { get; set; }

        public SortOptions? SortOptions { get; set; }
    } 
}
