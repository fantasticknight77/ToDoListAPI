using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Data.Models;

namespace ToDoListAPI.Services
{
    public class UserService : IUserService
    {
        //------------------------------------------------------------------------------
        /// Services to be injected from the dependency injection container
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<UserService> logger;



        /// <summary>
        /// Initializes a new instance of the <see cref="UserService" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        public UserService(
            ApplicationDbContext dbContext,
            ILogger<UserService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }


        /// <summary>
        /// Gets the users.
        /// </summary>
        public async Task<GetUsersResponse> GetUsers()
        {
            try
            {
                // Get available users
                var users = await dbContext.Users
                    .Where(x => x.Role != UserRoles.ADMIN)
                    .Select(x => new UserDTO
                    {
                        ID = x.ID,
                        Username = x.Username,
                        Email = x.Email,
                        Role = x.Role,
                        ConcurrencyToken = x.ConcurrencyToken,
                    })
                    .ToListAsync();



                logger.LogInformation("[{0}]: Get Users Successfully!", DateTime.UtcNow.ToString());

                return new GetUsersResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Get Users Successfully!",
                    Users = users
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Get Users Failed!", DateTime.UtcNow.ToString());

                return new GetUsersResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Get Users Failed!",
                };
            }
        }



        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task<GetUserResponse> GetUser(int id)
        {
            try
            {
                // Get user
                var user = await dbContext.Users
                    .Where(x => x.Role != UserRoles.ADMIN && x.ID == id)
                    .Select(x => new UserDTO
                    {
                        ID = x.ID,
                        Username = x.Username,
                        Email = x.Email,
                        Role = x.Role,
                        ConcurrencyToken = x.ConcurrencyToken,
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    logger.LogError("[{0}]: User Not Found!", DateTime.UtcNow.ToString());

                    return new GetUserResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "User Not Found!",
                    };
                }



                logger.LogInformation("[{0}]: Get User Successfully!", DateTime.UtcNow.ToString());

                return new GetUserResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Get User Successfully!",
                    User = user
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Get User Failed!", DateTime.UtcNow.ToString());

                return new GetUserResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Get User Failed!",
                };
            }
        }



        /// <summary> Updates the user role.</summary>
        /// <param name="id">The username.</param>
        /// <param name="role">The new role.</param>
        public async Task<ChangeUserRoleResponse> ChangeUserRole(int id, ChangeUserRoleRequest request)
        {
            try
            {
                // Find User
                var userToUpdate = await dbContext.Users.FirstOrDefaultAsync(x => x.ID == id && x.Username != "admin");

                if (userToUpdate == null)
                {
                    logger.LogError("[{0}]: User Not Found!", DateTime.UtcNow.ToString());

                    return new ChangeUserRoleResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "User Not Found!",
                    };
                }



                // Check role is exist
                if (request.Role != UserRoles.USER && request.Role != UserRoles.ADMIN)
                {
                    logger.LogError("[{0}]: Role Not Found!", DateTime.UtcNow.ToString());

                    return new ChangeUserRoleResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "Role Not Found!",
                    };
                }



                // Check role is same
                if (request.Role == userToUpdate.Role)
                {
                    logger.LogError("[{0}]: Role Is Same!", DateTime.UtcNow.ToString());

                    return new ChangeUserRoleResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "Role Is Same!",
                    };
                }



                // Update role
                try
                {
                    userToUpdate.ConcurrencyToken = request.ConcurrencyToken;
                    userToUpdate.Role = request.Role;

                    int updateUserRoleResult = await dbContext.SaveChangesAsync();

                    if (updateUserRoleResult == 0) throw new Exception("Change User Role Failed!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!dbContext.Users.Any(e => e.ID == id))
                    {
                        logger.LogError("[{0}]: User Not Found!", DateTime.UtcNow.ToString());

                        return new ChangeUserRoleResponse
                        {
                            Status = ResponseStatus.ERROR,
                            Message = "User Not Found!",
                        };
                    }
                    else
                    {
                        logger.LogError("[{0}]: User Is Updated By Others!", DateTime.UtcNow.ToString());

                        return new ChangeUserRoleResponse
                        {
                            Status = ResponseStatus.ERROR,
                            Message = "User Is Updated By Others!",
                        };
                    }
                }



                logger.LogInformation("[{0}]: Change User Role Successfully!", DateTime.UtcNow.ToString());

                return new ChangeUserRoleResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Change User Role Successfully!"
                };
            }         
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Change User Role Failed!", DateTime.UtcNow.ToString());

                return new ChangeUserRoleResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Change User Role Failed!",
                };
            }
        }
    }
}
