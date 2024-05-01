namespace notes.application.Constants
{
    public class AppConstants
    {
        public const string AppName = "Notes API";

        public const string AppVersion = "v1";

        public const int DefaultPageSize = 10;

        /// <summary>
        /// Password should contain one or more digit, lowercase letter, uppercase letter, special symbol.
        /// Password should have length between 8 and 16 inclusive.
        /// </summary>
        public const string PasswordRegex = "^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$";

        public static class Role
        {
            public const string User = "User";
        }
    }
}