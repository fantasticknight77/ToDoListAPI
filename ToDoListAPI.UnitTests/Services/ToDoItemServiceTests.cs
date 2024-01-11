using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoListAPI.Data.Models;
using ToDoListAPI.Data;
using ToDoListAPI.Services;
using Moq.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Protocol.Core.Types;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ToDoListAPI.UnitTests
{
    public class ToDoItemServiceTests
    {
        [Fact]
        public async void ToDoItemService_GetToDoItems_Success_WithoutFilterAndSort()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = null,
            };

            var result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get ToDo Items Successfully!", result.Message);
            Assert.Equal(3, result.ToDoItems.Count());
        }



        [Fact]
        public async void ToDoItemService_GetToDoItems_Success_WithAllFilter()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var testRequest = new GetToDoItemsRequest
            {
                FilterOptions = new FilterOptions
                {
                    Name = "Test",
                    Status = ToDoItemStatus.NOTSTARTED,
                    Priority = ToDoItemPriority.LOW,
                    Tag = "Test",
                    StartDate = DateTime.Parse("2024-07-08 14:40:52"),
                    EndDate = DateTime.Parse("2024-12-08 14:40:52"),
                },
                SortOptions = null,
            };

            var result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get ToDo Items Successfully!", result.Message);
            Assert.Single(result.ToDoItems);
            Assert.Equal("Test1", result.ToDoItems.First().Name);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItems_Success_WithAllSorter()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act (Test Name Sorter)
            var testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = new SortOptions
                {
                    SortConditionName = "Name",
                    SortConditionValue = SortOrder.DESC
                },
            };

            var result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get ToDo Items Successfully!", result.Message);
            Assert.Equal(3, result.ToDoItems.Count());
            Assert.Equal("Test3", result.ToDoItems.First().Name);


            // Act (Test Status Sorter)
            testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = new SortOptions
                {
                    SortConditionName = "Status",
                    SortConditionValue = SortOrder.ASC
                },
            };

            result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get ToDo Items Successfully!", result.Message);
            Assert.Equal(3, result.ToDoItems.Count());
            Assert.Equal("Test3", result.ToDoItems.First().Name);



            // Act (Test Priority Sorter)
            testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = new SortOptions
                {
                    SortConditionName = "Priority",
                    SortConditionValue = SortOrder.DESC
                },
            };

            result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get ToDo Items Successfully!", result.Message);
            Assert.Equal(3, result.ToDoItems.Count());
            Assert.Equal("Test2", result.ToDoItems.First().Name);



            // Act (Test DueDate Sorter)
            testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = new SortOptions
                {
                    SortConditionName = "DueDate",
                    SortConditionValue = SortOrder.ASC
                },
            };

            result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get ToDo Items Successfully!", result.Message);
            Assert.Equal(3, result.ToDoItems.Count());
            Assert.Equal("Test1", result.ToDoItems.First().Name);



            // Act (Test Other Sorter)
            testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = new SortOptions
                {
                    SortConditionName = "",
                    SortConditionValue = SortOrder.ASC
                },
            };

            result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get ToDo Items Successfully!", result.Message);
            Assert.Equal(3, result.ToDoItems.Count());
            Assert.Equal("Test1", result.ToDoItems.First().Name);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItems_Failed_JWTClaims()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var mockApplicationContext = MockApplicationDbContext();

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = null,
            };

            var result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("JWT Not Containing User Email!", result.Message);
            Assert.Empty(result.ToDoItems);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItems_Failed_UserNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = null,
            };

            var result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
            Assert.Empty(result.ToDoItems);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItems_Failed_ThrowError()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).Throws(new Exception());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var testRequest = new GetToDoItemsRequest
            {
                FilterOptions = null,
                SortOptions = null,
            };

            var result = await toDoItemService.GetToDoItems(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemsResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Get ToDo Items Failed!", result.Message);
            Assert.Empty(result.ToDoItems);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItem_Success()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.GetToDoItem(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get ToDo Item Successfully!", result.Message);
            Assert.NotNull(result.ToDoItem);
            Assert.Equal("Test1", result.ToDoItem.Name);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItem_Failed_UserNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.GetToDoItem(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
            Assert.Null(result.ToDoItem);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItem_Failed_ToDoItemNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.ToDoItems).ReturnsDbSet(new List<ToDoItem>());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.GetToDoItem(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Not Found!", result.Message);
            Assert.Null(result.ToDoItem);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItem_Failed_CannotAccessToDoItem()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.GetToDoItem(4);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("You Cannot Access This ToDo Item!", result.Message);
            Assert.Null(result.ToDoItem);
        }



        [Fact]
        public async void ToDoItemService_GetToDoItem_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).Throws(new Exception());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.GetToDoItem(4);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Get ToDo Item Failed!", result.Message);
            Assert.Null(result.ToDoItem);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Success()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1Modify",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.NOTSTARTED,
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Update ToDo Item Successfully!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_UserNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.NOTSTARTED,
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_ToDoItemNotExist()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.NOTSTARTED,
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(5, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Not Exist!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_CannotUpdateToDoItem()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.NOTSTARTED,
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(4, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("You Cannot Update This ToDo Item!", result.Message);
        }


        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_ToDoItemStatusNotCorrect()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = "Test",
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Status Not Correct, Only Accepts Not Started, In Progress or Completed!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_ToDoItemPriorityNotCorrect()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.COMPLETED,
                Priority = "Test",
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Priority Not Correct, Only Accepts Low, Medium or High!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_NotModified()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.NOTSTARTED,
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item And Tags Are Same!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_ConcurrencyExceptionToDoItemNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            List<ToDoItem> toDoItems = new List<ToDoItem>
            {
                new ToDoItem
                {
                    ID = 1,
                    Name = "Test1",
                    Description = "Test1",
                    DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                    Status = ToDoItemStatus.NOTSTARTED,
                    Priority = ToDoItemPriority.LOW,
                    UserID = 1,
                    User = new User
                    {
                        ID = 1,
                        Username = "Test1",
                        Email = "test1@example.com",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Role = UserRoles.USER,
                        ConcurrencyToken = passwordHash
                    },
                    ToDoItemTags = new List<ToDoItemTag>
                    {
                        new ToDoItemTag
                        {
                            ID = 1,
                            Name = "Test1",
                            ToDoItemID = 1,
                        }
                    },
                    ConcurrencyToken = passwordHash
                }
            };

            mockApplicationContext.SetupSequence(x => x.ToDoItems).ReturnsDbSet(toDoItems).ReturnsDbSet(new List<ToDoItem>());
            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new DbUpdateConcurrencyException());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1Modify",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.NOTSTARTED,
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Not Found!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_ConcurrencyExceptionToDoItemUpdated()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new DbUpdateConcurrencyException());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1Modify",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.NOTSTARTED,
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Is Updated By Others!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_UpdateToDoItem_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new Exception());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new UpdateToDoItemRequest
            {
                Name = "Test1Modify",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Status = ToDoItemStatus.NOTSTARTED,
                Priority = ToDoItemPriority.LOW,
                ConcurrencyToken = passwordHash,
                ToDoItemTags = new List<string>
                {
                    "Test1"
                }
            };

            var result = await toDoItemService.UpdateToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Update ToDo Item Failed!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_CreateToDoItem_Success()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new CreateToDoItemRequest
            {
                Name = "Test1Modify",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Priority = ToDoItemPriority.LOW,
            };

            var result = await toDoItemService.CreateToDoItem(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Create ToDo Item Successfully!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_CreateToDoItem_Failed_UserNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new CreateToDoItemRequest
            {
                Name = "Test1Modify",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Priority = ToDoItemPriority.LOW,
            };

            var result = await toDoItemService.CreateToDoItem(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_CreateToDoItem_Failed_PriorityNotCorrect()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new CreateToDoItemRequest
            {
                Name = "Test1Modify",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Priority = "Test",
            };

            var result = await toDoItemService.CreateToDoItem(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Priority Not Correct, Only Accepts Low, Medium or High!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_CreateToDoItem_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new Exception());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            var testRequest = new CreateToDoItemRequest
            {
                Name = "Test1Modify",
                Description = "Test1",
                DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                Priority = ToDoItemPriority.LOW,
            };

            var result = await toDoItemService.CreateToDoItem(testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreateToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Create ToDo Item Failed!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_DeleteToDoItem_Success()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.DeleteToDoItem(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<DeleteToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Delete ToDo Item Successfully!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_DeleteToDoItem_Failed_UserNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.DeleteToDoItem(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<DeleteToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_DeleteToDoItem_Failed_ToDoItemNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.ToDoItems).ReturnsDbSet(new List<ToDoItem>());

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.DeleteToDoItem(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<DeleteToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Not Found!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_DeleteToDoItem_Failed_CannotDeleteToDoItem()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.DeleteToDoItem(4);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<DeleteToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("You Cannot Delete This ToDo Item!", result.Message);
        }


        [Fact]
        public async void ToDoItemService_DeleteToDoItem_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new Exception());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.DeleteToDoItem(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<DeleteToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Delete ToDo Item Failed!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_GetAvailableUsersToShare_Success()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.GetAvailableUsersToShare(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetAvailableUsersToShareResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Get Available Users To Share Successfully!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_GetAvailableUsersToShare_Failed_ToDoItemNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.ToDoItems).ReturnsDbSet(new List<ToDoItem>());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.GetAvailableUsersToShare(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetAvailableUsersToShareResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDoItem Not Found!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_GetAvailableUsersToShare_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.ToDoItems).Throws(new Exception());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            var result = await toDoItemService.GetAvailableUsersToShare(1);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<GetAvailableUsersToShareResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Get Available Users To Share Failed!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_ShareToDoItem_Success()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            List<int> testRequest = new List<int> { 2 };

            var result = await toDoItemService.ShareToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShareToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.SUCCESS, result.Status);
            Assert.Equal("Share ToDo Item Successfully!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_ShareToDoItem_Failed_UserNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            List<int> testRequest = new List<int> { 2 };

            var result = await toDoItemService.ShareToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShareToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("User Not Found!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_ShareToDoItem_Failed_ToDoItemNotFound()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.ToDoItems).ReturnsDbSet(new List<ToDoItem>());

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            List<int> testRequest = new List<int> { 2 };

            var result = await toDoItemService.ShareToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShareToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("ToDo Item Not Found!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_ShareToDoItem_Failed_CannotShareToDoItem()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            List<int> testRequest = new List<int> { 2 };

            var result = await toDoItemService.ShareToDoItem(4, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShareToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("You Cannot Share This ToDo Item!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_ShareToDoItem_Failed_UserToSharedSame()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            List<int> testRequest = new List<int>();

            var result = await toDoItemService.ShareToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShareToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Users To Be Shared Are Same!", result.Message);
        }



        [Fact]
        public async void ToDoItemService_ShareToDoItem_Failed_ThrowException()
        {
            // Arrange
            var mockLogger = MockLogger();

            var mockHttpContextAccessor = MockHttpContextAccessor();

            var mockApplicationContext = MockApplicationDbContext();

            mockApplicationContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new Exception());

            ToDoItemService toDoItemService = new ToDoItemService(mockLogger.Object, mockApplicationContext.Object, mockHttpContextAccessor.Object);



            // Act
            List<int> testRequest = new List<int> { 2 };

            var result = await toDoItemService.ShareToDoItem(1, testRequest);



            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShareToDoItemResponse>(result);

            Assert.Equal(ResponseStatus.ERROR, result.Status);
            Assert.Equal("Share ToDo Item Failed!", result.Message);
        }



        private class MockDatabaseFacade : DatabaseFacade
        {
            public MockDatabaseFacade(DbContext context) : base(context)
            {
            }

            public override Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
                Task.FromResult(Mock.Of<IDbContextTransaction>());

        }



        private static Mock<ILogger<ToDoItemService>> MockLogger()
        {
            var mockLogger = new Mock<ILogger<ToDoItemService>>();

            return mockLogger;
        }



        private static Mock<IHttpContextAccessor> MockHttpContextAccessor()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = new DefaultHttpContext();

            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Email, "test1@example.com"),
            };

            context.Request.HttpContext.User.AddIdentity(new ClaimsIdentity(authClaims));

            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            return mockHttpContextAccessor;
        }



        private static Mock<ApplicationDbContext> MockApplicationDbContext()
        {
            AuthenticationService.CreatePasswordHash("Testing123*", out byte[] passwordHash, out byte[] passwordSalt);

            List<User> users = new List<User>
            {
                new User
                {
                    ID = 1,
                    Username = "Test1",
                    Email = "test1@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = UserRoles.USER,
                    ConcurrencyToken = passwordHash
                },
                new User
                {
                    ID = 2,
                    Username = "Test2",
                    Email = "test2@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = UserRoles.USER,
                    ConcurrencyToken = passwordHash
                }
            };

            List<ToDoItem> toDoItems = new List<ToDoItem>
            {
                new ToDoItem
                {
                    ID = 1,
                    Name = "Test1",
                    Description = "Test1",
                    DueDate = DateTime.Parse("2024-09-08 14:40:52"),
                    Status = ToDoItemStatus.NOTSTARTED,
                    Priority = ToDoItemPriority.LOW,
                    UserID = 1,
                    User = new User
                    {
                        ID = 1,
                        Username = "Test1",
                        Email = "test1@example.com",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Role = UserRoles.USER,
                        ConcurrencyToken = passwordHash
                    },
                    ToDoItemTags = new List<ToDoItemTag>
                    {
                        new ToDoItemTag
                        {
                            ID = 1,
                            Name = "Test1",
                            ToDoItemID = 1,
                        }
                    },
                    ConcurrencyToken = passwordHash
                },
                new ToDoItem
                {
                    ID = 2,
                    Name = "Test2",
                    Description = "Test2",
                    DueDate = DateTime.Parse("2024-07-08 14:40:52"),
                    Status = ToDoItemStatus.INPROGRESS,
                    Priority = ToDoItemPriority.MEDIUM,
                    UserID = 1,
                    User = new User
                    {
                        ID = 1,
                        Username = "Test1",
                        Email = "test1@example.com",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Role = UserRoles.USER,
                        ConcurrencyToken = passwordHash
                    },
                    ToDoItemTags = new List<ToDoItemTag>
                    {
                        new ToDoItemTag
                        {
                            ID = 2,
                            Name = "Test2",
                            ToDoItemID = 2,
                        }
                    },
                    ConcurrencyToken = passwordHash
                },
                new ToDoItem
                {
                    ID = 3,
                    Name = "Test3",
                    Description = "Test3",
                    DueDate = DateTime.Parse("2024-05-08 14:40:52"),
                    Status = ToDoItemStatus.COMPLETED,
                    Priority = ToDoItemPriority.HIGH,
                    UserID = 1,
                    User = new User
                    {
                        ID = 1,
                        Username = "Test1",
                        Email = "test1@example.com",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Role = UserRoles.USER,
                        ConcurrencyToken = passwordHash
                    },
                    ToDoItemTags = new List<ToDoItemTag>
                    {
                        new ToDoItemTag
                        {
                            ID = 3,
                            Name = "Test3",
                            ToDoItemID = 3,
                        }
                    },
                    ConcurrencyToken = passwordHash
                },
                new ToDoItem
                {
                    ID = 4,
                    Name = "Test4",
                    Description = "Test4",
                    DueDate = DateTime.Parse("2024-05-08 14:40:52"),
                    Status = ToDoItemStatus.COMPLETED,
                    Priority = ToDoItemPriority.HIGH,
                    UserID = 2,
                    User = new User
                    {
                        ID = 2,
                        Username = "Test2",
                        Email = "test2@example.com",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Role = UserRoles.USER,
                        ConcurrencyToken = passwordHash
                    },
                    ToDoItemTags = new List<ToDoItemTag>(),
                    ConcurrencyToken = passwordHash
                }
            };

            List<ToDoItemTag> toDoItemTags = new List<ToDoItemTag>
            {
                new ToDoItemTag
                {
                    ID = 1,
                    Name = "Test1",
                    ToDoItemID = 1,
                },
                new ToDoItemTag
                {
                    ID = 2,
                    Name = "Test2",
                    ToDoItemID = 2,
                },
                new ToDoItemTag
                {
                    ID = 3,
                    Name = "Test1",
                    ToDoItemID = 3,
                },
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test Database")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var mockApplicationContext = new Mock<ApplicationDbContext>(options);
            mockApplicationContext.Setup(x => x.Users).ReturnsDbSet(users);
            mockApplicationContext.Setup(x => x.ToDoItems).ReturnsDbSet(toDoItems);
            mockApplicationContext.Setup(x => x.ToDoItemTags).ReturnsDbSet(toDoItemTags);
            mockApplicationContext.Setup(x => x.ToDoItemUsers).ReturnsDbSet(new List<ToDoItemUser>());
            mockApplicationContext.SetupGet(x => x.Database).Returns(new MockDatabaseFacade(mockApplicationContext.Object));

            return mockApplicationContext;
        }
    }
}
