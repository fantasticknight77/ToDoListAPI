using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoListAPI.Data;
using ToDoListAPI.Data.Models;

namespace ToDoListAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        //------------------------------------------------------------------------------
        /// Services to be injected from the dependency injection container
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthenticationService> logger;
        private readonly ApplicationDbContext dbContext;



        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public AuthenticationService(
            IConfiguration configuration,
            ILogger<AuthenticationService> logger,
            ApplicationDbContext dbContext)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.dbContext = dbContext;
        }



        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                string jwt;

                // Check if user exists
                var userExists = await dbContext.Users
                    .Where(x => x.Email == request.Email)
                    .FirstOrDefaultAsync();

                if (userExists == null)
                {
                    logger.LogError("[{0}]: User Not Found!", DateTime.UtcNow.ToString());

                    return new LoginResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "User Not Found!",
                    };
                }

                // Login process
                if (VerifyPassword(request.Password, userExists.PasswordHash, userExists.PasswordSalt))
                {
                    var authClaims = new List<Claim> {
                        new Claim(ClaimTypes.Email, userExists.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("role", userExists.Role)
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

                    var token = new JwtSecurityToken(
                        issuer: configuration["JWT:ValidIssuer"],
                        audience: configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddMinutes(20),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    jwt = new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    throw new Exception("Login Failed!");
                }



                logger.LogInformation("[{0}]: Login Successfully!", DateTime.UtcNow.ToString());

                return new LoginResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Login Successfully!",
                    Token = jwt
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Login Failed!", DateTime.UtcNow.ToString());

                return new LoginResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Login Failed!",
                };
            }
        }



        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            try
            {
                // Check if user exists
                var userExists = await dbContext.Users
                    .Where(x => x.Email == request.Email)
                    .FirstOrDefaultAsync();

                if (userExists != null)
                {
                    logger.LogError("[{0}]: User Already Exists!", DateTime.UtcNow.ToString());

                    return new RegisterResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "User Already Exists!"
                    };
                };



                // Create User
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = new User()
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = UserRoles.USER
                };

                dbContext.Users.Add(user);

                int result = await dbContext.SaveChangesAsync();

                if (result == 0) throw new Exception("Register Failed!");



                logger.LogInformation("[{0}]: Register Successfully!", DateTime.UtcNow.ToString());

                return new RegisterResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "User Created Successfully!"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Register Failed!", DateTime.UtcNow.ToString());

                return new RegisterResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "User Creation Failed! Please Check User Details And Try Again."
                };
            }
        }



        /// <summary> Create Password Hash for store in database.</summary>
        /// <param name="password">The password of user.</param>
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }



        /// <summary> Verifies the password.</summary>
        /// <param name="password"> The password.</param>
        /// <param name="passwordHash"> The password hash.</param>
        /// <param name="passwordSalt"> The password salt.</param>
        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
