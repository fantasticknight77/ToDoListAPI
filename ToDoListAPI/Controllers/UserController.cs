using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    [Authorize(Roles = UserRoles.ADMIN)]
    public class UserController : ControllerBase
    {
        //------------------------------------------------------------------------------
        /// Services to be injected from the dependency injection container
        private readonly IUserService userService;



        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }



        // GET: api/user        
        /// <summary>
        /// Gets the users.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            GetUsersResponse response = await userService.GetUsers();

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // GET: api/user/5        
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            GetUserResponse response = await userService.GetUser(id);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // POST: api/user/change-role/5                
        /// <summary>
        /// Changes the user role.
        /// </summary>
        /// <param name="request">The request.</param>
        [HttpPost("change-role/{id}")]
        public async Task<IActionResult> ChangeUserRole([FromRoute] int id, [FromBody] ChangeUserRoleRequest request)
        {
            ChangeUserRoleResponse response = await userService.ChangeUserRole(id, request);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
