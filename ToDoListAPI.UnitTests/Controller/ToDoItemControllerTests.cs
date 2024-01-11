using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoListAPI.Controllers;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.UnitTests
{
    public class ToDoItemControllerTests
    {
        [Fact]
        public async void ToDoItemController_GetToDoItems_Ok()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            GetToDoItemsResponse mockGetToDoItemsResponse = new GetToDoItemsResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockToDoItemService.Setup(x => x.GetToDoItems(It.IsAny<GetToDoItemsRequest>())).Returns(Task.FromResult(mockGetToDoItemsResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.GetToDoItems(new GetToDoItemsRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<GetToDoItemsResponse>(item.Value);

            var responseItem = item.Value as GetToDoItemsResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_GetToDoItems_BadRequest()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            GetToDoItemsResponse mockGetToDoItemsResponse = new GetToDoItemsResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockToDoItemService.Setup(x => x.GetToDoItems(It.IsAny<GetToDoItemsRequest>())).Returns(Task.FromResult(mockGetToDoItemsResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.GetToDoItems(new GetToDoItemsRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<GetToDoItemsResponse>(item.Value);

            var responseItem = item.Value as GetToDoItemsResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_GetToDoItem_Ok()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            GetToDoItemResponse mockGetToDoItemResponse = new GetToDoItemResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockToDoItemService.Setup(x => x.GetToDoItem(It.IsAny<int>())).Returns(Task.FromResult(mockGetToDoItemResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.GetToDoItem(0);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<GetToDoItemResponse>(item.Value);

            var responseItem = item.Value as GetToDoItemResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_GetToDoItem_BadRequest()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            GetToDoItemResponse mockGetToDoItemResponse = new GetToDoItemResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockToDoItemService.Setup(x => x.GetToDoItem(It.IsAny<int>())).Returns(Task.FromResult(mockGetToDoItemResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.GetToDoItem(0);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<GetToDoItemResponse>(item.Value);

            var responseItem = item.Value as GetToDoItemResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_UpdateToDoItem_Ok()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            UpdateToDoItemResponse mockUpdateToDoItemsResponse = new UpdateToDoItemResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockToDoItemService.Setup(x => x.UpdateToDoItem(It.IsAny<int>(), It.IsAny<UpdateToDoItemRequest>())).Returns(Task.FromResult(mockUpdateToDoItemsResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.UpdateToDoItem(0, new UpdateToDoItemRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<UpdateToDoItemResponse>(item.Value);

            var responseItem = item.Value as UpdateToDoItemResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_UpdateToDoItem_BadRequest()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            UpdateToDoItemResponse mockUpdateToDoItemsResponse = new UpdateToDoItemResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockToDoItemService.Setup(x => x.UpdateToDoItem(It.IsAny<int>(), It.IsAny<UpdateToDoItemRequest>())).Returns(Task.FromResult(mockUpdateToDoItemsResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.UpdateToDoItem(0, new UpdateToDoItemRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<UpdateToDoItemResponse>(item.Value);

            var responseItem = item.Value as UpdateToDoItemResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_CreateToDoItem_Ok()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            CreateToDoItemResponse mockCreateToDoItemsResponse = new CreateToDoItemResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockToDoItemService.Setup(x => x.CreateToDoItem(It.IsAny<CreateToDoItemRequest>())).Returns(Task.FromResult(mockCreateToDoItemsResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.CreateToDoItem(new CreateToDoItemRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<CreateToDoItemResponse>(item.Value);

            var responseItem = item.Value as CreateToDoItemResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_CreateToDoItem_BadRequest()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            CreateToDoItemResponse mockCreateToDoItemsResponse = new CreateToDoItemResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockToDoItemService.Setup(x => x.CreateToDoItem(It.IsAny<CreateToDoItemRequest>())).Returns(Task.FromResult(mockCreateToDoItemsResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.CreateToDoItem(new CreateToDoItemRequest());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<CreateToDoItemResponse>(item.Value);

            var responseItem = item.Value as CreateToDoItemResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_DeleteToDoItem_Ok()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            DeleteToDoItemResponse mockDeleteToDoItemsResponse = new DeleteToDoItemResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockToDoItemService.Setup(x => x.DeleteToDoItem(It.IsAny<int>())).Returns(Task.FromResult(mockDeleteToDoItemsResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.DeleteToDoItem(0);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<DeleteToDoItemResponse>(item.Value);

            var responseItem = item.Value as DeleteToDoItemResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_DeleteToDoItem_BadRequest()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            DeleteToDoItemResponse mockDeleteToDoItemsResponse = new DeleteToDoItemResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockToDoItemService.Setup(x => x.DeleteToDoItem(It.IsAny<int>())).Returns(Task.FromResult(mockDeleteToDoItemsResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.DeleteToDoItem(0);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<DeleteToDoItemResponse>(item.Value);

            var responseItem = item.Value as DeleteToDoItemResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_GetAvailableUsersToShare_Ok()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            GetAvailableUsersToShareResponse mockGetAvailableUsersToShareResponse = new GetAvailableUsersToShareResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockToDoItemService.Setup(x => x.GetAvailableUsersToShare(It.IsAny<int>())).Returns(Task.FromResult(mockGetAvailableUsersToShareResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.GetAvailableUsersToShare(0);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<GetAvailableUsersToShareResponse>(item.Value);

            var responseItem = item.Value as GetAvailableUsersToShareResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_GetAvailableUsersToShare_BadRequest()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            GetAvailableUsersToShareResponse mockGetAvailableUsersToShareResponse = new GetAvailableUsersToShareResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockToDoItemService.Setup(x => x.GetAvailableUsersToShare(It.IsAny<int>())).Returns(Task.FromResult(mockGetAvailableUsersToShareResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.GetAvailableUsersToShare(0);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<GetAvailableUsersToShareResponse>(item.Value);

            var responseItem = item.Value as GetAvailableUsersToShareResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_ShareToDoItem_Ok()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            ShareToDoItemResponse mockShareToDoItemResponse = new ShareToDoItemResponse
            {
                Status = ResponseStatus.SUCCESS
            };

            mockToDoItemService.Setup(x => x.ShareToDoItem(It.IsAny<int>(), It.IsAny<List<int>>())).Returns(Task.FromResult(mockShareToDoItemResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.ShareToDoItem(0, new List<int>());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var item = result as OkObjectResult;
            Assert.IsType<ShareToDoItemResponse>(item.Value);

            var responseItem = item.Value as ShareToDoItemResponse;
            Assert.Equal(ResponseStatus.SUCCESS, responseItem.Status);
        }



        [Fact]
        public async void ToDoItemController_ShareToDoItem_BadRequest()
        {
            // Arrange
            var mockToDoItemService = new Mock<IToDoItemService>();

            ShareToDoItemResponse mockShareToDoItemResponse = new ShareToDoItemResponse
            {
                Status = ResponseStatus.ERROR
            };

            mockToDoItemService.Setup(x => x.ShareToDoItem(It.IsAny<int>(), It.IsAny<List<int>>())).Returns(Task.FromResult(mockShareToDoItemResponse));

            ToDoItemController todoItemController = new ToDoItemController(mockToDoItemService.Object);



            // Act
            var result = await todoItemController.ShareToDoItem(0, new List<int>());



            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var item = result as BadRequestObjectResult;
            Assert.IsType<ShareToDoItemResponse>(item.Value);

            var responseItem = item.Value as ShareToDoItemResponse;
            Assert.Equal(ResponseStatus.ERROR, responseItem.Status);
        }
    }
}
