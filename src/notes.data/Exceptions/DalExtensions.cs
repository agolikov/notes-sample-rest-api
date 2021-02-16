using System;

namespace notes.data.Exceptions
{
    public static class DalExtensions
    {
        public static DalException EntityNotFoundException(this Guid id)
        {
            return new DalException(ErrorCodes.EntityNotFound, id);
        }

        public static DalException VersionInNotCorrectException(this Guid id)
        {
            return new DalException(ErrorCodes.VersionIsNotCorrect, id);
        }
    }
}
