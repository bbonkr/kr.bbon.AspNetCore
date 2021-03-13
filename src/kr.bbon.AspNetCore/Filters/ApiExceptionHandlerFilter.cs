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
    public class ApiExceptionHandlerFilter : ExceptionFilterAttribute, IExceptionFilter, IAsyncExceptionFilter
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
            var statusCode = HttpStatusCode.InternalServerError;
            var statusCodeValue = (int)statusCode; 

            context.HttpContext.Response.StatusCode = statusCodeValue;

            if (context.Exception is HttpStatusException)
            {
                var actual = context.Exception as HttpStatusException;
                statusCodeValue = (int)actual.StatusCode;

                context.HttpContext.Response.StatusCode = statusCodeValue;
                actionResult = new ObjectResult(ApiResponseModelFactory.Create(actual.StatusCode, actual.GetDetails()));
            }

            if(context.Exception is SomethingWrongException)
            {
                var actual = context.Exception as SomethingWrongException;

                context.HttpContext.Response.StatusCode = statusCodeValue;
                actionResult = new ObjectResult(ApiResponseModelFactory.Create(statusCode, actual.Message, actual.GetDetails()));
            }

            context.Result = actionResult ?? new ObjectResult(new ApiResponseModel
            {
                StatusCode = statusCodeValue,
                Message = context.Exception.Message,
            });
        }
    }
}
