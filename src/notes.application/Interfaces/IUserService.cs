using notes.application.Models.User;
using System;
using System.Threading.Tasks;

namespace notes.application.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetUserAsync(Guid userId);

        Task<UserModel> ChangeUserPasswordAsync(ChangePasswordModel model);
    }
}