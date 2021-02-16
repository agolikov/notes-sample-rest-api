using notes.application.Models;
using notes.application.Models.User;
using System.Threading.Tasks;

namespace notes.application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LogInResult> SignInAsync(SignInModel model);

        Task<UserModel> SignUpAsync(SignUpModel model);
    }
}
