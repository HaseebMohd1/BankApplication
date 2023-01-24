using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace ExceptionHandler;
public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
           => applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
}
