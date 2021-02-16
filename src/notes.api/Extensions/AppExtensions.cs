using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using notes.api.Middleware;
using notes.api.Models;
using System;
using System.Linq;
using System.Net;

namespace notes.api.Extensions
{
    public static class AppExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }

        public static Guid GetSessionUserId(this HttpContext context)
        {
            return Guid.Parse(context?.User.Identity?.Name);
        }

        public static ActionResult GetErrorResponse(this ActionContext context)
        {
            var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage).ToArray();

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return new BadRequestObjectResult(
                new ApiErrorResponse
                {
                    Message = "ValidationError",
                    Details = errors,
                    ResponseCode = HttpStatusCode.BadRequest.ToString()
                });
        }
    }
}