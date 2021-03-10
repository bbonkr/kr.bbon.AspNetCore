using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using kr.bbon.AspNetCore.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace kr.bbon.AspNetCore.Filters
{
    public class ApiExceptionHandler : ExceptionFilterAttribute, IExceptionFilter, IAsyncExceptionFilter
    {
        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            HandleException(context);

            return Task.CompletedTask;
        }

        private void HandleException(ExceptionContext context)
        {
            ObjectResult actionResult = null;
            var statusCode = (int)HttpStatusCode.InternalServerError; 
            context.HttpContext.Response.StatusCode = statusCode;

            if (context.Exception is HttpStatusException)
            {
                var actual = context.Exception as HttpStatusException;
                statusCode = (int)actual.StatusCode;

                context.HttpContext.Response.StatusCode = statusCode;
                actionResult = new ObjectResult(ApiResponseModelFactory.Create(actual.StatusCode, actual.GetDetails()));
            }

            context.Result = actionResult ?? new ObjectResult(new ApiResponseModel
            {
                StatusCode = statusCode,
                Message = context.Exception.Message,
            });
        }
    }
}
