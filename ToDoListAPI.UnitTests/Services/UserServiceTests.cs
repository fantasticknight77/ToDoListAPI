using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.UnitTests
{
    public class UserServiceTests
    {
        [Fact]
        public async void UserService_GetUsers_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            var result = await userService.GetUsers();



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetUsersResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get Users Successfully!", result.Message);

            Assert.Single(result.Users);
            Assert.Equal("Test", result.Users.First().Username);
        }



        [Fact]
        public async void UserService_GetUsers_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).Throws(new Exception());

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            var result = await userService.GetUsers();



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetUsersResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Get Users Failed!", result.Message);
        }



        [Fact]
        public async void UserService_GetUser_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            var result = await userService.GetUser(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetUserResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get User Successfully!", result.Message);

            Assert.NotNull(result.User);
            Assert.Equal("Test", result.User.Username);
        }



        [Fact]
        public async void UserService_GetUser_Failed_UserNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            var result = await userService.GetUser(2);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetUserResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
        }



        [Fact]
        public async void UserService_GetUser_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).Throws(new Exception());

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            var result = await userService.GetUser(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetUserResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Get User Failed!", result.Message);
        }



        [Fact]
        public async void UserService_ChangeUserRole_Success()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

			AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

			List<User> users = new List<User>
			{
				new User
				{
					ID = 1,
					Username = "Test",
					Email = "test@example.com",
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt,
					Role = UserRoles.USER,
					ConcurrencyToken = passwordHash
				}
			};

			mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(users);

			UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            ChangeUserRoleRequest testRequest = new ChangeUserRoleRequest
            {
                Role = UserRoles.ADMIN,
                ConcurrencyToken = passwordHash
            };

            var result = await userService.ChangeUserRole(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChangeUserRoleResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Change User Role Successfully!", result.Message);
        }



        [Fact]
        public async void UserService_ChangeUserRole_Failed_UserNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            ChangeUserRoleRequest testRequest = new ChangeUserRoleRequest
            {
                Role = UserRoles.ADMIN,
                ConcurrencyToken = passwordHash
            };

            var result = await userService.ChangeUserRole(2, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChangeUserRoleResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
        }



        [Fact]
        public async void UserService_ChangeUserRole_Failed_RoleNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            ChangeUserRoleRequest testRequest = new ChangeUserRoleRequest
            {
                Role = "Leader",
                ConcurrencyToken = passwordHash
            };

            var result = await userService.ChangeUserRole(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChangeUserRoleResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Role Not Found!", result.Message);
        }



        [Fact]
        public async void UserService_ChangeUserRole_Failed_RoleIsSame()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            ChangeUserRoleRequest testRequest = new ChangeUserRoleRequest
            {
                Role = UserRoles.USER,
                ConcurrencyToken = passwordHash
            };

            var result = await userService.ChangeUserRole(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChangeUserRoleResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Role Is Same!", result.Message);
        }



        [Fact]
        public async void UserService_ChangeUserRole_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new Exception());

			AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

			List<User> users = new List<User>
			{
				new User
				{
					ID = 1,
					Username = "Test",
					Email = "test@example.com",
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt,
					Role = UserRoles.USER,
					ConcurrencyToken = passwordHash
				}
			};

			mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(users);

			UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            ChangeUserRoleRequest testRequest = new ChangeUserRoleRequest
            {
                Role = UserRoles.ADMIN,
                ConcurrencyToken = passwordHash
            };

            var result = await userService.ChangeUserRole(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChangeUserRoleResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Change User Role Failed!", result.Message);
        }



        [Fact]
        public async void UserService_ChangeUserRole_Failed_ConcurrencyExceptionUserNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            List<User> users = new List<User>
            {
                new User
                {
                    ID = 1,
                    Username = "Test",
                    Email = "test@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = UserRoles.USER,
                    ConcurrencyToken = passwordHash
                }
            };

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.SetupSequence(x => x.Users).ReturnsDbSet(users).ReturnsDbSet(new List<User>());

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new DbUpdateConcurrencyException());

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            ChangeUserRoleRequest testRequest = new ChangeUserRoleRequest
            {
                Role = UserRoles.ADMIN,
                ConcurrencyToken = passwordHash
            };

            var result = await userService.ChangeUserRole(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChangeUserRoleResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
        }



        [Fact]
        public async void UserService_ChangeUserRole_Failed_ConcurrencyExceptionUserUpdated()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new DbUpdateConcurrencyException());

            UserService userService = new UserService(mockApplicationContext.Object, mockLogger.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            ChangeUserRoleRequest testRequest = new ChangeUserRoleRequest
            {
                Role = UserRoles.ADMIN,
                ConcurrencyToken = passwordHash
            };

            var result = await userService.ChangeUserRole(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChangeUserRoleResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Is Updated By Others!", result.Message);
        }



        private static Mock<ApplicationDbContext> MockApplicationDbContext()
        {
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            List<User> users = new List<User>
            {
                new User
                {
                    ID = 1,
                    Username = "Test",
                    Email = "test@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = UserRoles.USER,
                    ConcurrencyToken = passwordHash
                }
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .Options;

            var mockApplicationContext = new Mock<ApplicationDbContext>(options);
            mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(users);

            return mockApplicationContext;
        }
    }
}
