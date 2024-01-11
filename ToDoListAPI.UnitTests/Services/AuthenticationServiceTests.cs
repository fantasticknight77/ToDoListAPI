using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.UnitTests
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public async void AuthenticationService_Login_Success()
        {
            // Arrange
            IConfiguration mockConfiguration = MockConfiguration();

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var mockApplicationContext = MockApplicationDbContext();

            AuthenticationService authenticationService = new AuthenticationService(mockConfiguration, mockLogger.Object, mockApplicationContext.Object);



            // Act
            LoginRequest testRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "Testing123*"
            };

            var result = await authenticationService.Login(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<LoginResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Login Successfully!", result.Message);
            Assert.NotEqual("", result.Token);
        }



        [Fact]
        public async void AuthenticationService_Login_UserNotFound()
        {
            // Arrange
            IConfiguration mockConfiguration = MockConfiguration();

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var mockApplicationContext = MockApplicationDbContext();

            AuthenticationService authenticationService = new AuthenticationService(mockConfiguration, mockLogger.Object, mockApplicationContext.Object);



            // Act
            LoginRequest testRequest = new LoginRequest
            {
                Email = "test1@example.com",
                Password = "Testing123*"
            };

            var result = await authenticationService.Login(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<LoginResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
            Assert.Equal("", result.Token);
        }



        [Fact]
        public async void AuthenticationService_Login_Failed()
        {
            // Arrange
            IConfiguration mockConfiguration = MockConfiguration();

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var mockApplicationContext = MockApplicationDbContext();

            AuthenticationService authenticationService = new AuthenticationService(mockConfiguration, mockLogger.Object, mockApplicationContext.Object);



            // Act
            LoginRequest testRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "Testing456*"
            };

            var result = await authenticationService.Login(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<LoginResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Login Failed!", result.Message);
            Assert.Equal("", result.Token);
        }



        [Fact]
        public async void AuthenticationService_Register_Success()
        {
            // Arrange
            IConfiguration mockConfiguration = MockConfiguration();

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            AuthenticationService authenticationService = new AuthenticationService(mockConfiguration, mockLogger.Object, mockApplicationContext.Object);



            // Act
            RegisterRequest testRequest = new RegisterRequest
            {
                Username = "Test",
                Email = "test1@example.com",
                Password = "Testing123*"
            };

            var result = await authenticationService.Register(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<RegisterResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("User Created Successfully!", result.Message);
        }



        [Fact]
        public async void AuthenticationService_Register_UserAlreadyExists()
        {
            // Arrange
            IConfiguration mockConfiguration = MockConfiguration();

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            AuthenticationService authenticationService = new AuthenticationService(mockConfiguration, mockLogger.Object, mockApplicationContext.Object);



            // Act
            RegisterRequest testRequest = new RegisterRequest
            {
                Username = "Test",
                Email = "test@example.com",
                Password = "Testing123*"
            };

            var result = await authenticationService.Register(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<RegisterResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Already Exists!", result.Message);
        }



        [Fact]
        public async void AuthenticationService_Register_Failed()
        {
            // Arrange
            IConfiguration mockConfiguration = MockConfiguration();

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            AuthenticationService authenticationService = new AuthenticationService(mockConfiguration, mockLogger.Object, mockApplicationContext.Object);



            // Act
            RegisterRequest testRequest = new RegisterRequest
            {
                Username = "Test1",
                Email = "test1@example.com",
                Password = "Testing123*"
            };

            var result = await authenticationService.Register(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<RegisterResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Creation Failed! Please Check User Details And Try Again.", result.Message);
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



        private static IConfiguration MockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"JWT:Secret", "1D.;{ytk/xn/pt0Ue6>.i3!vJq0>£9A`"},
                {"JWT:ValidIssuer", "ValidIssuer"},
                {"JWT:ValidAudience", "Validaudience"},
            };

            IConfiguration mockConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return mockConfiguration;
        }
    }
}
