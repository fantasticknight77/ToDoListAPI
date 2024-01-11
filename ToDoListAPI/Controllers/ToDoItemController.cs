using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [Authorize]
    [Route("api/v1/todoitem")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        //------------------------------------------------------------------------------
        /// Services to be injected from the dependency injection container
        private readonly IToDoItemService toDoItemService;



        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemController"/> class.
        /// </summary>
        /// <param name="toDoItemService">To do item service.</param>
        public ToDoItemController(IToDoItemService toDoItemService)
        {
            this.toDoItemService = toDoItemService;
        }



        // GET: api/todoitem        
        /// <summary>
        /// Gets todo items.
        /// </summary>
        /// <param name="request">The request.</param>
        [HttpPost("get-todo-items")]
        public async Task<IActionResult> GetToDoItems([FromBody] GetToDoItemsRequest request)
        {
            GetToDoItemsResponse response = await toDoItemService.GetToDoItems(request);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response); 
            }

            return BadRequest(response);
        }



        // GET: api/todoitem/5        
        /// <summary>
        /// Gets todo item.
        /// </summary>
        /// <param name="id">The id of todo item.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoItem([FromRoute] int id)
        {
            GetToDoItemResponse response = await toDoItemService.GetToDoItem(id);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // PUT: api/todoitem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754        
        /// <summary>
        /// Updates todo item.
        /// </summary>
        /// <param name="id">The id of todo item.</param>
        /// <param name="request">The request.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoItem([FromRoute] int id, [FromBody] UpdateToDoItemRequest request)
        {
            UpdateToDoItemResponse response = await toDoItemService.UpdateToDoItem(id, request);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // POST: api/todoitem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754        
        /// <summary>
        /// Creates todo item.
        /// </summary>
        /// <param name="request">The request.</param>
        [HttpPost]
        public async Task<IActionResult> CreateToDoItem([FromBody] CreateToDoItemRequest request)
        {
            CreateToDoItemResponse response = await toDoItemService.CreateToDoItem(request);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // DELETE: api/todoitem/5        
        /// <summary>
        /// Deletes todo item.
        /// </summary>
        /// <param name="id">The id of the todo item.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem([FromRoute] int id)
        {
            DeleteToDoItemResponse response = await toDoItemService.DeleteToDoItem(id);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // GET: api/todoitem/get-available-users/5        
        /// <summary>
        /// Gets the available users to share.
        /// </summary>
        [HttpGet("get-available-users/{id}")]
        public async Task<IActionResult> GetAvailableUsersToShare([FromRoute] int id)
        {
            GetAvailableUsersToShareResponse response = await toDoItemService.GetAvailableUsersToShare(id);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // POST: api/todoitem/share/5        
        /// <summary>
        /// Shares todo item.
        /// </summary>
        /// <param name="id">The id of the todo item.</param>
        /// <param name="request">The request.</param>
        [HttpPost("share/{id}")]
        public async Task<IActionResult> ShareToDoItem([FromRoute] int id, [FromBody] List<int> request)
        {
            ShareToDoItemResponse response = await toDoItemService.ShareToDoItem(id, request);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
