using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoListAPI.Controllers;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.UnitTests
{
    public class UserControllerTests
    {
        [Fact]
        public async void UserController_GetUsers_Ok()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();

            GetUsersResponse mockGetUsersResponse = new GetUsersResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockUserService.Setup(x => x.GetUsers()).Returns(Task.FromResult(mockGetUsersResponse));

            UserController userController = new UserController(mockUserService.Object);



            // Act
            var result = await userController.GetUsers();



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<GetUsersResponse>(item.Value);

            var responseItem = item.Value as GetUsersResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void UserController_GetUsers_BadRequest()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();

            GetUsersResponse mockGetUsersResponse = new GetUsersResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockUserService.Setup(x => x.GetUsers()).Returns(Task.FromResult(mockGetUsersResponse));

            UserController userController = new UserController(mockUserService.Object);



            // Act
            var result = await userController.GetUsers();



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<GetUsersResponse>(item.Value);

            var responseItem = item.Value as GetUsersResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void UserController_GetUser_Ok()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();

            GetUserResponse mockGetUserResponse = new GetUserResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockUserService.Setup(x => x.GetUser(It.IsAny<int>())).Returns(Task.FromResult(mockGetUserResponse));

            UserController userController = new UserController(mockUserService.Object);



            // Act
            var result = await userController.GetUser(0);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<GetUserResponse>(item.Value);

            var responseItem = item.Value as GetUserResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void UserController_GetUser_BadRequest()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();

            GetUserResponse mockGetUserResponse = new GetUserResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockUserService.Setup(x => x.GetUser(It.IsAny<int>())).Returns(Task.FromResult(mockGetUserResponse));

            UserController userController = new UserController(mockUserService.Object);



            // Act
            var result = await userController.GetUser(0);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<GetUserResponse>(item.Value);

            var responseItem = item.Value as GetUserResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void UserController_ChangeUserRole_Ok()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();

            ChangeUserRoleResponse mockChangeUserRoleResponse = new ChangeUserRoleResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockUserService.Setup(x => x.ChangeUserRole(It.IsAny<int>(), It.IsAny<ChangeUserRoleRequest>())).Returns(Task.FromResult(mockChangeUserRoleResponse));

            UserController userController = new UserController(mockUserService.Object);



            // Act
            var result = await userController.ChangeUserRole(0, new ChangeUserRoleRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<ChangeUserRoleResponse>(item.Value);

            var responseItem = item.Value as ChangeUserRoleResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void UserController_ChangeUserRole_BadRequest()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();

            ChangeUserRoleResponse mockChangeUserRoleResponse = new ChangeUserRoleResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockUserService.Setup(x => x.ChangeUserRole(It.IsAny<int>(), It.IsAny<ChangeUserRoleRequest>())).Returns(Task.FromResult(mockChangeUserRoleResponse));

            UserController userController = new UserController(mockUserService.Object);



            // Act
            var result = await userController.ChangeUserRole(0, new ChangeUserRoleRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<ChangeUserRoleResponse>(item.Value);

            var responseItem = item.Value as ChangeUserRoleResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }
    }
}
