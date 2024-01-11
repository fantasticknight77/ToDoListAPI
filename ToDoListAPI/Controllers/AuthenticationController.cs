using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [Route("api/v1/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //------------------------------------------------------------------------------
        /// Services to be injected from the dependency injection container
        private readonly IAuthenticationService authenticationService;



        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public AuthenticationController(IAuthenticationService authenticationService) {
            this.authenticationService = authenticationService;
        }



        // api/authentication/login        
        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <param name="request">The request.</param>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            LoginResponse response = await authenticationService.Login(request);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // api/authentication/register        
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="request">The request.</param>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            RegisterResponse response = await authenticationService.Register(request);

            if (response.Status == ResponseStatus.SUCCESS)
            {
                return Ok(response); 
            }
                
            return BadRequest(response);
        }
    }
}
