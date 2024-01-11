using ToDoListAPI.Data.Models;

namespace ToDoListAPI.Services
{
    public interface IUserService
    {
        Task<GetUsersResponse> GetUsers();

        Task<GetUserResponse> GetUser(int id);

        Task<ChangeUserRoleResponse> ChangeUserRole(int id, ChangeUserRoleRequest request);
    }
}
