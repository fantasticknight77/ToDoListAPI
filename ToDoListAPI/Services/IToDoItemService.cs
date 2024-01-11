using ToDoListAPI.Data.Models;

namespace ToDoListAPI.Services
{
    public interface IToDoItemService
    {
        Task<GetToDoItemsResponse> GetToDoItems(GetToDoItemsRequest request);

        Task<GetToDoItemResponse> GetToDoItem(int id);

        Task<UpdateToDoItemResponse> UpdateToDoItem(int id, UpdateToDoItemRequest request);

        Task<CreateToDoItemResponse> CreateToDoItem(CreateToDoItemRequest request);

        Task<DeleteToDoItemResponse> DeleteToDoItem(int id);

        Task<GetAvailableUsersToShareResponse> GetAvailableUsersToShare(int toDoItemID);

        Task<ShareToDoItemResponse> ShareToDoItem(int id, List<int> userIDsToShare);
    }
}
