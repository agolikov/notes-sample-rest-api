using System;
using System.Net;
using System.Reflection;
using System.Resources;

namespace notes.application.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Code { get; }
        public string EntityId { get; set; }
        public string Details { get; set; }
        private string[] MessageParams { get; }

        public AppException(string code = null, string entityId = null, string details = null,
            params string[] messageParams)
        {
            StatusCode = HttpStatusCode.BadRequest;
            Code = code;
            EntityId = entityId;
            MessageParams = messageParams;
            Details = details;
        }

        public string GetFormattedMessage()
        {
            var rm = new ResourceManager("Strings", Assembly.GetExecutingAssembly());

            String messageFormat = rm.GetString(Code);

            if (messageFormat == null)
            {
                messageFormat = rm.GetString(Code);
            }

            return string.Format(messageFormat, MessageParams);
        }
    }
}
