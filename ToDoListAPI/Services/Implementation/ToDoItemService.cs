using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using System.Security.Claims;
using ToDoListAPI.Data;
using ToDoListAPI.Data.Models;

namespace ToDoListAPI.Services
{
    public class ToDoItemService : IToDoItemService
    {
        //------------------------------------------------------------------------------
        /// Services to be injected from the dependency injection container
        private readonly ILogger<ToDoItemService> logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;



        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public ToDoItemService(
            ILogger<ToDoItemService> logger,
            ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }



        /// <summary>
        /// Gets todo items.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task<GetToDoItemsResponse> GetToDoItems(GetToDoItemsRequest request)
        {
            try
            {
                // Validate user
                ValidateUserResponse validateUserResponse = await ValidateUser();

                if (validateUserResponse.Status == ResponseStatus.ERROR)
                {
                    return new GetToDoItemsResponse
                    {
                        Status = validateUserResponse.Status,
                        Message = validateUserResponse.Message,
                    };
                }



                // Initialize Base Query
                IQueryable<ToDoItem> toDoItems = dbContext.ToDoItems
                    .Include(x => x.User)
                    .Include(x => x.ToDoItemTags)
                    .Select(x => new ToDoItem
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description,
                        DueDate = x.DueDate,
                        Status = x.Status,
                        Priority = x.Priority,
                        UserID = x.UserID,
                        User = new User
                        {
                            ID = x.UserID,
                            Username = x.User!.Username
                        },
                        ToDoItemTags = x.ToDoItemTags,
                    })
                    .AsNoTracking();



                // Application with user role can only access todo items related to the user.
                if (validateUserResponse.User!.Role == UserRoles.USER)
                {
                    var toDoItemsIDsBelongToUser = await dbContext.ToDoItemUsers
                        .Where(x => x.UserID == validateUserResponse.User!.ID)
                        .Select(x => x.ToDoItemID)
                        .ToListAsync();

                    toDoItems = toDoItems
                        .Where(x => x.UserID == validateUserResponse.User!.ID || toDoItemsIDsBelongToUser.Contains(x.ID));
                }



                // Handle Filter & Sorting
                if (request.FilterOptions == null && request.SortOptions == null)
                {
                    logger.LogInformation("[{0}]: Get ToDo Items Successfully!", DateTime.UtcNow.ToString());

                    return new GetToDoItemsResponse
                    {
                        Status = ResponseStatus.SUCCESS,
                        Message = "Get ToDo Items Successfully!",
                        ToDoItems = await toDoItems
                            .Select(x => new ToDoItemDTO
                            {
                                ID = x.ID,
                                Name = x.Name,
                                Description = x.Description,
                                DueDate = x.DueDate,
                                Status = x.Status,
                                Priority = x.Priority,
                                UserID = x.UserID,
                                Username = x.User!.Username,
                                ToDoItemTagNames = x.ToDoItemTags != null ? x.ToDoItemTags.Select(tag => tag.Name).ToList() : new List<string>(),
                            })
                            .ToListAsync()
                    };
                }

                if (request.FilterOptions != null)
                {
                    toDoItems = HandleFiltering(request.FilterOptions, toDoItems);
                }

                if (request.SortOptions != null)
                {
                    toDoItems = HandleSorting(request.SortOptions, toDoItems);
                }



                logger.LogInformation("[{0}]: Get ToDo Items Successfully!", DateTime.UtcNow.ToString());

                return new GetToDoItemsResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Get ToDo Items Successfully!",
                    ToDoItems = await toDoItems
                        .Select(x => new ToDoItemDTO
                        {
                            ID = x.ID,
                            Name = x.Name,
                            Description = x.Description,
                            DueDate = x.DueDate,
                            Status = x.Status,
                            Priority = x.Priority,
                            UserID = x.UserID,
                            Username = x.User!.Username,
                            ToDoItemTagNames = x.ToDoItemTags != null ? x.ToDoItemTags.Select(tag => tag.Name).ToList() : new List<string>(),
                        })
                        .ToListAsync()
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Get ToDo Items Failed!", DateTime.UtcNow.ToString());

                return new GetToDoItemsResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Get ToDo Items Failed!",
                };
            }   
        }



        /// <summary>
        /// Gets todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task<GetToDoItemResponse> GetToDoItem(int id)
        {
            try
            {
                // Validate user
                ValidateUserResponse validateUserResponse = await ValidateUser();

                if (validateUserResponse.Status == ResponseStatus.ERROR) 
                {
                    return new GetToDoItemResponse
                    {
                        Status = validateUserResponse.Status,
                        Message = validateUserResponse.Message,
                    };
                }



                // Initialize base query
                var toDoItem = await dbContext.ToDoItems
                    .Include(x => x.ToDoItemTags)
                    .Where(x => x.ID == id)
                    .FirstOrDefaultAsync();

                if (toDoItem == null)
                {
                    logger.LogError("[{0}]: ToDo Item Not Found!", DateTime.UtcNow.ToString());

                    return new GetToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "ToDo Item Not Found!",
                    };
                }



                // Application with user role can only access todo items related to the user.
                var toDoItemsIDsBelongToUser = await dbContext.ToDoItemUsers
                        .Where(x => x.UserID == validateUserResponse.User!.ID)
                        .Select(x => x.ToDoItemID)
                        .ToListAsync();

                if (validateUserResponse.User!.Role == UserRoles.USER 
                    && toDoItem.UserID != validateUserResponse.User!.ID 
                    && !toDoItemsIDsBelongToUser.Contains(id))
                {
                    logger.LogError("[{0}]: You Cannot Access This ToDo Item!", DateTime.UtcNow.ToString());

                    return new GetToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "You Cannot Access This ToDo Item!",
                    };
                }



                logger.LogInformation("[{0}]: Get ToDoItem Successfully!", DateTime.UtcNow.ToString());

                return new GetToDoItemResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Get ToDo Item Successfully!",
                    ToDoItem = new ToDoItemDTO
                    {
                        ID = toDoItem.ID,
                        Name = toDoItem.Name,
                        Description = toDoItem.Description,
                        DueDate = toDoItem.DueDate,
                        Status = toDoItem.Status,
                        Priority = toDoItem.Priority,
                        UserID = toDoItem.UserID,
                        Username = toDoItem.User!.Username,
                        ToDoItemTagNames = toDoItem.ToDoItemTags != null ? toDoItem.ToDoItemTags.Select(tag => tag.Name).ToList() : new List<string>(),
                        ConcurrencyToken = toDoItem.ConcurrencyToken,
                    }

                };
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Get ToDo Item Failed!", DateTime.UtcNow.ToString());

                return new GetToDoItemResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Get ToDo Item Failed!",
                };
            }
        }



        /// <summary>
        /// Updates todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        public async Task<UpdateToDoItemResponse> UpdateToDoItem(int id, UpdateToDoItemRequest request)
        {
            try
            {
                // Validate user
                ValidateUserResponse validateUserResponse = await ValidateUser();

                if (validateUserResponse.Status == ResponseStatus.ERROR)
                {
                    return new UpdateToDoItemResponse
                    {
                        Status = validateUserResponse.Status,
                        Message = validateUserResponse.Message,
                    };
                }



                // Update ToDo Item
                ToDoItem? toDoItemToUpdate = await dbContext.ToDoItems.Where(x => x.ID == id).FirstOrDefaultAsync();

                if (toDoItemToUpdate == null)
                {
                    logger.LogError("[{0}]: ToDo Item Not Exist!", DateTime.UtcNow.ToString());

                    return new UpdateToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "ToDo Item Not Exist!",
                    };
                }



                // Application with user role can only access todo items related to the user.
                var toDoItemsIDsBelongToUser = await dbContext.ToDoItemUsers
                        .Where(x => x.UserID == validateUserResponse.User!.ID)
                        .Select(x => x.ToDoItemID)
                        .ToListAsync();

                if (validateUserResponse.User!.Role == UserRoles.USER 
                    && toDoItemToUpdate.UserID != validateUserResponse.User!.ID 
                    && !toDoItemsIDsBelongToUser.Contains(id))
                {
                    logger.LogError("[{0}]: You Cannot Update This ToDo Item!", DateTime.UtcNow.ToString());

                    return new UpdateToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "You Cannot Update This ToDo Item!",
                    };
                }



                // Validate request
                if (request.Status != ToDoItemStatus.NOTSTARTED && request.Status != ToDoItemStatus.INPROGRESS && request.Status != ToDoItemStatus.COMPLETED)
                {
                    logger.LogError("[{0}]: ToDo Item Status Not Correct, Only Accepts Not Started, In Progress or Completed!", DateTime.UtcNow.ToString());

                    return new UpdateToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "ToDo Item Status Not Correct, Only Accepts Not Started, In Progress or Completed!",
                    };
                }

                if (request.Priority != ToDoItemPriority.LOW && request.Priority != ToDoItemPriority.MEDIUM && request.Priority != ToDoItemPriority.HIGH)
                {
                    logger.LogError("[{0}]: ToDo Item Priority Not Correct, Only Accepts Low, Medium or High!", DateTime.UtcNow.ToString());

                    return new UpdateToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "ToDo Item Priority Not Correct, Only Accepts Low, Medium or High!",
                    };
                }



                // Update todo item
                using var dbContextTransaction = dbContext.Database.BeginTransaction();
                
                try
                {
                    bool toDoItemModified = CheckToDoItemModified(toDoItemToUpdate, request);

                    // Get tooo item tags exists
                    var toDoItemTagsExist = await dbContext.ToDoItemTags
                        .Where(x => x.ToDoItemID == toDoItemToUpdate.ID)
                        .AsNoTracking()
                        .ToListAsync();

                    // Validate request
                    // Check todo item and tags both not modified
                    if (toDoItemTagsExist.Count == request.ToDoItemTags.Count
                        && !toDoItemTagsExist.Select(x => x.Name).Except(request.ToDoItemTags).Any()
                        && !toDoItemModified)
                    {
                        logger.LogError("[{0}]: ToDo Item And Tags Are Same!", DateTime.UtcNow.ToString());

                        return new UpdateToDoItemResponse
                        {
                            Status = ResponseStatus.ERROR,
                            Message = "ToDo Item And Tags Are Same!",
                        };
                    }



                    // Update todo item
                    if (toDoItemModified)
                    {
                        toDoItemToUpdate.ConcurrencyToken = request.ConcurrencyToken;

                        toDoItemToUpdate.Name = request.Name;
                        toDoItemToUpdate.Description = request.Description;
                        toDoItemToUpdate.DueDate = request.DueDate;
                        toDoItemToUpdate.Status = request.Status;
                        toDoItemToUpdate.Priority = request.Priority;
                    
                        int updateToDoItemResult = await dbContext.SaveChangesAsync();

                        if (updateToDoItemResult == 0) throw new Exception("Update ToDo Item Failed!");
                    }



                    // Update todo item tags
                    if (toDoItemTagsExist.Count != request.ToDoItemTags.Count
                        || toDoItemTagsExist.Select(x => x.Name).Except(request.ToDoItemTags).Any())
                    {
                        var toDoItemToAdd = new List<ToDoItemTag>();
                        var toDoItemToDelete = new List<ToDoItemTag>();

                        foreach (var todoItemTag in toDoItemTagsExist)
                        {
                            if (request.ToDoItemTags.Contains(todoItemTag.Name))
                            {
                                request.ToDoItemTags.Remove(todoItemTag.Name);
                            }
                            else
                            {
                                toDoItemToDelete.Add(todoItemTag);
                            }
                        }

                        foreach (var toDoItemTagToBeAdd in request.ToDoItemTags)
                        {
                            ToDoItemTag toDoItemTag = new ToDoItemTag()
                            {
                                ToDoItemID = toDoItemToUpdate.ID,
                                Name = toDoItemTagToBeAdd,
                            };

                            toDoItemToAdd.Add(toDoItemTag);
                        }

                        dbContext.ToDoItemTags.AddRange(toDoItemToAdd);
                        dbContext.ToDoItemTags.RemoveRange(toDoItemToDelete);

                        int updateToDoItemTagsResult = await dbContext.SaveChangesAsync();

                        if (updateToDoItemTagsResult == 0) throw new Exception("Update ToDo Item Tags Failed!");
                    }   



                    // Commit Transation
                    await dbContextTransaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!dbContext.ToDoItems.Any(e => e.ID == id))
                    {
                        logger.LogError("[{0}]: ToDo Item Not Found!", DateTime.UtcNow.ToString());

                        return new UpdateToDoItemResponse
                        {
                            Status = ResponseStatus.ERROR,
                            Message = "ToDo Item Not Found!",
                        };
                    }
                    else
                    {
                        logger.LogError("[{0}]: ToDo Item Is Updated By Others!", DateTime.UtcNow.ToString());

                        return new UpdateToDoItemResponse
                        {
                            Status = ResponseStatus.ERROR,
                            Message = "ToDo Item Is Updated By Others!",
                        };
                    }
                }      



                logger.LogInformation("[{0}]: Update ToDo Item Successfully!", DateTime.UtcNow.ToString());

                return new UpdateToDoItemResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Update ToDo Item Successfully!",
                };
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Update ToDo Item Failed!", DateTime.UtcNow.ToString());

                return new UpdateToDoItemResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Update ToDo Item Failed!",
                };
            }
        }



        /// <summary>
        /// Creates todo item.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task<CreateToDoItemResponse> CreateToDoItem(CreateToDoItemRequest request)
        {
            try
            {
                // Validate user
                ValidateUserResponse validateUserResponse = await ValidateUser();

                if (validateUserResponse.Status == ResponseStatus.ERROR)
                {
                    return new CreateToDoItemResponse
                    {
                        Status = validateUserResponse.Status,
                        Message = validateUserResponse.Message,
                    };
                }



                // Validate request
                if (request.Priority != ToDoItemPriority.LOW && request.Priority != ToDoItemPriority.MEDIUM && request.Priority != ToDoItemPriority.HIGH)
                {
                    logger.LogError("[{0}]: ToDo Item Priority Not Correct, Only Accepts Low, Medium or High!", DateTime.UtcNow.ToString());

                    return new CreateToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "ToDo Item Priority Not Correct, Only Accepts Low, Medium or High!",
                    };
                }



                // Create new todo item
                ToDoItem newItem = new ToDoItem
                {
                    Name = request.Name,
                    Description = request.Description,
                    DueDate = request.DueDate,
                    Status = ToDoItemStatus.NOTSTARTED,
                    Priority = request.Priority,
                    UserID = validateUserResponse.User!.ID
                };

                dbContext.ToDoItems.Add(newItem);

                int createToDoItemResult = await dbContext.SaveChangesAsync();

                if (createToDoItemResult == 0) throw new Exception("Create ToDo Item Failed!");



                logger.LogInformation("[{0}]: Create ToDo Item Successfully!", DateTime.UtcNow.ToString());

                return new CreateToDoItemResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Create ToDo Item Successfully!",
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Create ToDo Item Failed!", DateTime.UtcNow.ToString());

                return new CreateToDoItemResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Create ToDo Item Failed!",
                };
            }
        }



        /// <summary>
        /// Deletes todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task<DeleteToDoItemResponse> DeleteToDoItem(int id)
        {
            try
            {
                // Validate user
                ValidateUserResponse validateUserResponse = await ValidateUser();

                if (validateUserResponse.Status == ResponseStatus.ERROR)
                {
                    return new DeleteToDoItemResponse
                    {
                        Status = validateUserResponse.Status,
                        Message = validateUserResponse.Message,
                    };
                }



                // Get todo item to delete
                ToDoItem? toDoItemToDelete = await dbContext.ToDoItems.Where(x => x.ID == id).FirstOrDefaultAsync();
                if (toDoItemToDelete == null)
                {
                    logger.LogError("[{0}]: ToDo Item Not Found!", DateTime.UtcNow.ToString());

                    return new DeleteToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "ToDo Item Not Found!",
                    };
                }



                // Application with user role can only access todo items related to the user.
                if (validateUserResponse.User!.Role == UserRoles.USER 
                    && toDoItemToDelete.UserID != validateUserResponse.User!.ID)
                {
                    logger.LogError("[{0}]: You Cannot Delete This ToDo Item!", DateTime.UtcNow.ToString());

                    return new DeleteToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "You Cannot Delete This ToDo Item!",
                    };
                }



                // Delete todo item tags
                using var dbContextTransaction = dbContext.Database.BeginTransaction();

                List<ToDoItemTag> toDoItemTagsToDelete = await dbContext.ToDoItemTags
                    .Where(x => x.ToDoItemID == toDoItemToDelete.ID)
                    .ToListAsync();

                if (toDoItemTagsToDelete.Count != 0)
                {
                    dbContext.ToDoItemTags.RemoveRange(toDoItemTagsToDelete);

                    int deleteToDoItemTagsResult = await dbContext.SaveChangesAsync();

                    if (deleteToDoItemTagsResult == 0) throw new Exception("Delete ToDo Item Tags Failed!");
                }



                // Delete related user
                List<ToDoItemUser> toDoItemUsersToDelete = await dbContext.ToDoItemUsers
                    .Where(x => x.ToDoItemID == toDoItemToDelete.ID)
                    .ToListAsync();

                if (toDoItemUsersToDelete.Count != 0)
                {
                    dbContext.ToDoItemUsers.RemoveRange(toDoItemUsersToDelete);

                    int deleteToDoItemUserResult = await dbContext.SaveChangesAsync();

                    if (deleteToDoItemUserResult == 0) throw new Exception("Delete ToDo Item Users Failed!");
                }



                // Delete todo item
                dbContext.ToDoItems.Remove(toDoItemToDelete);

                int deleteToDoItemResult = await dbContext.SaveChangesAsync();

                if (deleteToDoItemResult == 0) throw new Exception("Delete ToDo Item Failed!");



                // Commit all the changes
                await dbContextTransaction.CommitAsync();



                logger.LogInformation("[{0}]: Delete ToDo Item Successfully!", DateTime.UtcNow.ToString());

                return new DeleteToDoItemResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Delete ToDo Item Successfully!",
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Delete ToDo Item Failed!", DateTime.UtcNow.ToString());

                return new DeleteToDoItemResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Delete ToDo Item Failed!",
                };
            }
        }



        /// <summary>
        /// Gets the available users to share.
        /// </summary>
        public async Task<GetAvailableUsersToShareResponse> GetAvailableUsersToShare(int toDoItemID)
        {
            try
            {
                // Get available users (exclude owner)
                var ownerIDOfToDoItem = await dbContext.ToDoItems
                    .Where(x => x.ID == toDoItemID)
                    .Select(x => x.UserID)
                    .FirstOrDefaultAsync();

                if (ownerIDOfToDoItem == 0)
                {
                    logger.LogError("[{0}]: ToDoItem Not Found!", DateTime.UtcNow.ToString());

                    return new GetAvailableUsersToShareResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "ToDoItem Not Found!",
                    };
                }

                var availableUsers = await dbContext.Users
                    .Where(x => x.ID != ownerIDOfToDoItem)
                    .Select(x => new AvailableUserToShare
                    {
                        UserID = x.ID,
                        Username = x.Username,
                    })
                    .ToListAsync();



                logger.LogInformation("[{0}]: Get Available Users To Share Successfully!", DateTime.UtcNow.ToString());

                return new GetAvailableUsersToShareResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Get Available Users To Share Successfully!",
                    Users = availableUsers
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Get Available Users To Share Failed!", DateTime.UtcNow.ToString());

                return new GetAvailableUsersToShareResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Get Available Users To Share Failed!",
                };
            }       
        }



        /// <summary>
        /// Shares todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userIDsToShare">The user i ds to share.</param>
        public async Task<ShareToDoItemResponse> ShareToDoItem(int id, List<int> userIDsToShare)
        {
            try
            {
                // Validate user
                ValidateUserResponse validateUserResponse = await ValidateUser();

                if (validateUserResponse.Status == ResponseStatus.ERROR)
                {
                    return new ShareToDoItemResponse
                    {
                        Status = validateUserResponse.Status,
                        Message = validateUserResponse.Message,
                    };
                }



                // Get todo item to share
                ToDoItem? toDoItemToShare = await dbContext.ToDoItems.Where(x => x.ID == id).FirstOrDefaultAsync();

                if (toDoItemToShare == null)
                {
                    logger.LogError("[{0}]: ToDo Item Not Found!", DateTime.UtcNow.ToString());

                    return new ShareToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "ToDo Item Not Found!",
                    };
                }



                // Application with user role can only access todo items related to the user.
                if (toDoItemToShare.UserID != validateUserResponse.User!.ID)
                {
                    logger.LogError("[{0}]: You Cannot Share This ToDo Item!", DateTime.UtcNow.ToString());

                    return new ShareToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "You Cannot Share This ToDo Item!",
                    };
                }             



                // Update share users
                var usersToAdd = new List<ToDoItemUser>();
                var usersToRemove = new List<ToDoItemUser>();

                var currentSharedUserIDs = await dbContext.ToDoItemUsers
                    .Where(x => x.ToDoItemID == id
                        && x.UserID != validateUserResponse.User!.ID)
                    .ToListAsync();



                // Check if two list are same
                if (currentSharedUserIDs.Count == userIDsToShare.Count &&
                    !currentSharedUserIDs.Select(x => x.UserID).ToList().Except(userIDsToShare).Any())
                {
                    logger.LogError("[{0}]: Users To Be Shared Are Same!", DateTime.UtcNow.ToString());

                    return new ShareToDoItemResponse
                    {
                        Status = ResponseStatus.ERROR,
                        Message = "Users To Be Shared Are Same!",
                    };
                }



                // Get users to unshare todo item
                foreach (var user in currentSharedUserIDs)
                {
                    if (userIDsToShare.Contains(user.UserID))
                    {
                        userIDsToShare.Remove(user.UserID);
                    }
                    else
                    {
                        usersToRemove.Add(user);
                    }
                }

                // Get users to share todo item
                foreach (var userID in userIDsToShare) 
                { 
                    ToDoItemUser userToShare = new ToDoItemUser
                    {
                        ToDoItemID = toDoItemToShare.ID,
                        UserID = userID 
                    };

                    usersToAdd.Add(userToShare);
                }

                dbContext.ToDoItemUsers.RemoveRange(usersToRemove);
                dbContext.ToDoItemUsers.AddRange(usersToAdd);

                int shareToDoItemResult = await dbContext.SaveChangesAsync();

                if (shareToDoItemResult == 0) throw new Exception("Share ToDo Item Failed");



                logger.LogInformation("[{0}]: Share ToDo Item Successfully!", DateTime.UtcNow.ToString());

                return new ShareToDoItemResponse
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Share ToDo Item Successfully!",
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[{0}]: Share ToDo Item Failed!", DateTime.UtcNow.ToString());

                return new ShareToDoItemResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Share ToDo Item Failed!",
                };
            }
        }



        /// <summary>
        /// Validates the user.
        /// </summary>
        private async Task<ValidateUserResponse> ValidateUser()
        {
            // Get Current User
            string? currentUserEmail = GetUserEmailInJWT();

            if (currentUserEmail == null)
            {
                logger.LogError("[{0}]: JWT Not Containing User Email!", DateTime.UtcNow.ToString());

                return new ValidateUserResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "JWT Not Containing User Email!",
                };
            }

            User? user = await dbContext.Users
                .Where(x => x.Email == currentUserEmail)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                logger.LogError("[{0}]: User Not Found!", DateTime.UtcNow.ToString());

                return new ValidateUserResponse
                {
                    Status = ResponseStatus.ERROR,
                    Message = "User Not Found!",
                };
            }


            logger.LogInformation("[{0}]: Validate User Successfully!", DateTime.UtcNow.ToString());

            return new ValidateUserResponse
            {
                Status = ResponseStatus.SUCCESS,
                Message = "Validate User Successfully!",
                User = user,
            };
        }



        /// <summary>
        /// Gets the user email in JWT.
        /// </summary>
        private string? GetUserEmailInJWT()
        {
            if (httpContextAccessor.HttpContext != null)
            {
                return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }

            return null;
        }



        /// <summary>
        /// Handles the filtering.
        /// </summary>
        /// <param name="filterOptions">The filter options.</param>
        /// <param name="toDoItems">To do items.</param>
        private static IQueryable<ToDoItem> HandleFiltering(FilterOptions filterOptions, IQueryable<ToDoItem> toDoItems)
        {
            List<Expression<Func<ToDoItem, bool>>> expressions = new List<Expression<Func<ToDoItem, bool>>>();
            IQueryable<ToDoItem> filteredToDoItems = toDoItems;

            // Filter name
            if (filterOptions.Name != null) expressions.Add(x => x.Name.Contains(filterOptions.Name));

            // Filter Status
            if (filterOptions.Status != null) expressions.Add(x => x.Status == filterOptions.Status);

            // Filter Priority
            if (filterOptions.Priority != null) expressions.Add(x => x.Priority == filterOptions.Priority);

            // Filter tag
            if (filterOptions.Tag != null) expressions.Add(x => x.ToDoItemTags != null && x.ToDoItemTags.Any(t => t.Name.Contains(filterOptions.Tag)));

            // Filter start date
            if (filterOptions.StartDate != null) expressions.Add(x => x.DueDate >= filterOptions.StartDate);

            // Filter end date
            if (filterOptions.EndDate != null) expressions.Add(x => x.DueDate <= filterOptions.EndDate);

            foreach (var expression in expressions)
            {
                filteredToDoItems = filteredToDoItems.Where(expression);
            }

            return filteredToDoItems;
        }



        /// <summary>
        /// Handles the sorting.
        /// </summary>
        /// <param name="sortOptions">The sort options.</param>
        /// <param name="toDoItems">To do items.</param>
        private static IQueryable<ToDoItem> HandleSorting(SortOptions sortOptions, IQueryable<ToDoItem> toDoItems)
        {
            Expression<Func<ToDoItem, object>>? expression = null;
            IQueryable<ToDoItem> sortedToDoItems = toDoItems;

            switch (sortOptions.SortConditionName)
            {
                case "Name":
                    expression = x => x.Name;
                    break;
                case "Status":
                    expression = x => x.Status;
                    break;
                case "Priority":
                    expression = x => x.Priority;
                    break;
                case "Duedate":
                    expression = x => x.DueDate;
                    break;
                default:
                    break;
            }

            if (expression == null) 
            {
                return sortedToDoItems;
            }

            if (sortOptions.SortConditionValue == SortOrder.ASC)
            {
                sortedToDoItems = sortedToDoItems.OrderBy(expression);
            }
            else if (sortOptions.SortConditionValue == SortOrder.DESC)
            {
                sortedToDoItems = sortedToDoItems.OrderByDescending(expression);
            }

            return sortedToDoItems;
        }



        /// <summary>
        /// Checks todo item modified or not.
        /// </summary>
        /// <param name="toDoItem">ToDoo item.</param>
        /// <param name="request">The request.</param>
        private bool CheckToDoItemModified(ToDoItem toDoItem, UpdateToDoItemRequest request)
        {
            bool modified = false;

            if (toDoItem.Name != request.Name) modified = true;
            if (toDoItem.Description != request.Description) modified = true;
            if (toDoItem.Status != request.Status) modified = true;
            if (toDoItem.DueDate != request.DueDate) modified = true;
            if (toDoItem.Priority != request.Priority) modified = true;

            return modified;
        }
    } 
}
