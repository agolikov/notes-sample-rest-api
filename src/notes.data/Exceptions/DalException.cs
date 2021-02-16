using System;

namespace notes.data.Exceptions
{
    public class DalException : Exception
    {
        public string Code { get; }
        public Guid? EntityId { get; set; }

        public DalException(string code = null, Guid? entityId = null)
        {
            Code = code;
            EntityId = entityId;
        }
    }
}
