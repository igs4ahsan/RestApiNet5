
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RestApiNet5.Exceptions
{

    public class RecordNotFoundException : Exception
    {
    }

    public class DbRecordValidationFailedException : Exception
    {
        public DbRecordValidationFailedException(ICollection<ValidationResult> errors)
        {
            ValidationErrors = errors;
        }

        public ICollection<ValidationResult> ValidationErrors { get; }
    }

    public static class MyExceptionHandler
    {
        public static void UseMyExceptionHandler(this IApplicationBuilder app)
        {

            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsync("Not Found");
                }
            });

            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    //if (exFeature?.Error is DbRecordValidationFailedException)
                    //{
                    //    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    //    await context.Response.WriteAsync(JsonConvert.SerializeObject(exFeature.Error));
                    //}

                    if (exFeature?.Error is RecordNotFoundException)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }

                    if (exFeature?.Error is UnauthorizedAccessException)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }

                    if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }

                });
            });
        }
    }
}