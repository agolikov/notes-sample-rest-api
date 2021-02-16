using notes.application.Models.User;

namespace notes.application.Models
{
    public class LogInResult
    {
        public UserModel User { get; set; }
        public string Token { get; set; }
    }
}
