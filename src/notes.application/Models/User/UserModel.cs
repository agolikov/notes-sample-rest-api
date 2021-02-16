using notes.application.Models.Common;

namespace notes.application.Models.User
{
    public class UserModel : CommonModel
    {
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
    }
}
