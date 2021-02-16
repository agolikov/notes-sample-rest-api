using notes.application.Exceptions;
using notes.data.Entities;

namespace notes.application.Extensions
{
    public static class ExceptionHelper
    {

        public static AppException EntityNotFoundException(this string userId)
        {
            return new AppException(ErrorCodes.EntityNotFound, userId);
        }

        public static AppException PasswordIsIncorrectException(this User user)
        {
            return new AppException(ErrorCodes.UserPasswordIsIncorrect, user.Email);
        }
        public static AppException EmailNotFoundException(this string email)
        {
            return new AppException(ErrorCodes.EmailNotFound, email);
        }

        public static AppException EmailAlreadyTakenException(this User user)
        {
            return new AppException(ErrorCodes.UserEmailAlreadyTaken, user.Email);
        }
    }
}
