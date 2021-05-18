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

            if (context.Exception is HttpStatusException httpStatusException)
            {
                statusCodeValue = (int)httpStatusException.StatusCode;                
                actionResult = new ObjectResult(ApiResponseModelFactory.Create(httpStatusException.StatusCode, httpStatusException.GetDetails()));
            }

            if(context.Exception is SomethingWrongException somethingWrongException)
            {                
                actionResult = new ObjectResult(ApiResponseModelFactory.Create(statusCode, somethingWrongException.Message, somethingWrongException.GetDetails()));
            }

            if (context.Exception is AggregateException aggregateException)
            {
                var innerErrors = aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count > 0
                        ? aggregateException.InnerExceptions.Select((inner, index) => new ErrorModel
                        {
                            Code = $"Inner Error {index + 1}",
                            Message = inner.Message,
                        }).ToList()
                        : new List<ErrorModel>();

                actionResult = new ObjectResult(ApiResponseModelFactory.Create(statusCodeValue, aggregateException.Message, new ErrorModel
                {
                    Code = "HTTP 500",
                    Message = aggregateException.Message,
                    InnerError = innerErrors.FirstOrDefault(),
                    InnerErrors = innerErrors,
                }));
            }

            if (actionResult == null)
            {
                var innerErrors = context.Exception.InnerException != null
                    ? new List<ErrorModel>{
                        new ErrorModel
                        {
                            Code = "Inner exception",
                            Message = context.Exception.InnerException.Message,
                        },
                    }
                    : new List<ErrorModel>();


                actionResult = new ObjectResult(ApiResponseModelFactory.Create(statusCodeValue, context.Exception.Message, new ErrorModel
                {
                    Code = "HTTP 500",
                    Message = context.Exception.Message,
                    InnerError = innerErrors.FirstOrDefault(),
                    InnerErrors = innerErrors,
                }));
            }

            context.HttpContext.Response.StatusCode = statusCodeValue;
            context.Result = actionResult;
        }
    }
}
