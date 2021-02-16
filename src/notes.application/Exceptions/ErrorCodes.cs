namespace notes.application.Exceptions
{
    public class ErrorCodes
    {
        public const string EntityNotFound = "EntityNotFound";
        public const string UserEmailAlreadyTaken = "UserEmailAlreadyTaken";
        public const string EmailNotFound = "EmailNotFound";
        public const string UserPasswordIsIncorrect = "UserPasswordIsIncorrect";
        public const string EmailFormatIsNotValid = "EmailFormatIsNotValid";
        public const string PasswordFormatIsNotValid = "PasswordFormatIsNotValid";
        public const string TagsCountIsLarge = "TagsCountIsLarge";
        public const string UnexpectedError = "UnexpectedError";
    }
}