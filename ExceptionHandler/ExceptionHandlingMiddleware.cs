using System.Net;
using Microsoft.AspNetCore.Http;

namespace ExceptionHandler;

    /// <summary>
    ///     This is a Custom Middleware to handle Errors Globally for all Exceptions at any project
    /// </summary>
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }

