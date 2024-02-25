
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Net.Http;
using System.Text.Json;

namespace KineMartAPI.Exceptions
{
    public static class ApplicationBuilderExtension
    {
        public static void AddGlobalErrorHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appErr =>
            {
                appErr.Run(async context =>
                {
                    string status, message, path;
                    var loggerFactory = new LoggerFactory();
                    var logger = loggerFactory.CreateLogger("AddGlobalErrorHandler");
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();

                    if (contextFeature != null)
                    {
                        if (contextFeature.GetType() == typeof(NotFoundException))
                        {
                            status = "NOT_FOUND";
                            message = contextFeature.Error.Message;
                            path = contextRequest!.Path;
                            context.Response.StatusCode = 404;
                        }
                        else if (contextFeature.GetType() == typeof(UniqueException) || contextFeature.GetType() 
                                                                                       == typeof(ExceptionBase))
                        {
                            status = "BAD_REQUEST";
                            message = contextFeature.Error.Message;
                            path = contextRequest!.Path;
                            context.Response.StatusCode = 400;
                        }
                        else
                        {
                            status = "BAD_REQUEST";
                            message = contextFeature.Error.Message;
                            path = contextRequest!.Path;
                            context.Response.StatusCode = 400;
                        }
                        var exceptionResult = JsonSerializer.Serialize(new { status, message,path });
                        logger.LogError(exceptionResult);
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(exceptionResult);
                    }

                });
            });
        }
            
    }
}
