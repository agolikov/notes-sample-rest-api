using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using notes.api.Models;
using notes.application.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace notes.api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;

        public ErrorHandlingMiddleware(RequestDelegate nextMiddleware)
        {
            _nextMiddleware = nextMiddleware;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _nextMiddleware(context);
            }
            catch (Exception appException)
            {
                await HandleExceptionAsync(context, appException);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ApiErrorResponse errorResponse = new ApiErrorResponse();

            HttpStatusCode responseCode = HttpStatusCode.InternalServerError;
            switch (exception)
            {
                case AppException appException:
                    responseCode = appException.StatusCode;
                    errorResponse.ResponseCode = appException.StatusCode.ToString();
                    errorResponse.EntityId = appException.EntityId;
                    errorResponse.Message = appException.GetFormattedMessage();
                    errorResponse.Details = new[] { appException.Details };
                    break;
                default:
                    errorResponse.ResponseCode = responseCode.ToString();
                    errorResponse.Message = exception.Message;
                    break;
            }

            var result = JsonConvert.SerializeObject(errorResponse, Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)responseCode;

            return context.Response.WriteAsync(result);
        }
    }
}
