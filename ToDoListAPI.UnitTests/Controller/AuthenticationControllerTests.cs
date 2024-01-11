using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoListAPI.Controllers;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.UnitTests
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public async void AuthenticationController_Login_Ok()
        {
            // Arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            LoginResponse mockLoginResponse = new LoginResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockAuthenticationService.Setup(x => x.Login(It.IsAny<LoginRequest>())).Returns(Task.FromResult(mockLoginResponse));

            AuthenticationController authenticationController = new AuthenticationController(mockAuthenticationService.Object);



            // Act
            var result = await authenticationController.Login(new LoginRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<LoginResponse>(item.Value);

            var responseItem = item.Value as LoginResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void AuthenticationController_Login_BadRequest()
        {
            // Arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            LoginResponse mockLoginResponse = new LoginResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockAuthenticationService.Setup(x => x.Login(It.IsAny<LoginRequest>())).Returns(Task.FromResult(mockLoginResponse));

            AuthenticationController authenticationController = new AuthenticationController(mockAuthenticationService.Object);



            // Act
            var result = await authenticationController.Login(new LoginRequest());



            // Assert
            Assert.NotNull(result);

            Console.WriteLine(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<LoginResponse>(item.Value);

            var responseItem = item.Value as LoginResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void AuthenticationController_Register_Ok()
        {
            // Arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            RegisterResponse mockRegisterResponse = new RegisterResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockAuthenticationService.Setup(x => x.Register(It.IsAny<RegisterRequest>())).Returns(Task.FromResult(mockRegisterResponse));

            AuthenticationController authenticationController = new AuthenticationController(mockAuthenticationService.Object);



            // Act
            var result = await authenticationController.Register(new RegisterRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<RegisterResponse>(item.Value);

            var responseItem = item.Value as RegisterResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void AuthenticationController_Register_BadRequest()
        {
            // Arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            RegisterResponse mockRegisterResponse = new RegisterResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockAuthenticationService.Setup(x => x.Register(It.IsAny<RegisterRequest>())).Returns(Task.FromResult(mockRegisterResponse));

            AuthenticationController authenticationController = new AuthenticationController(mockAuthenticationService.Object);



            // Act
            var result = await authenticationController.Register(new RegisterRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<RegisterResponse>(item.Value);

            var responseItem = item.Value as RegisterResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }
    }
}
