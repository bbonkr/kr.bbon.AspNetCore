using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using kr.bbon.Core;
using kr.bbon.AspNetCore.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using kr.bbon.Core.Models;

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
            
            object responseModel = null;            

            var statusCode = HttpStatusCode.InternalServerError;
            var statusCodeValue = (int)statusCode;

            var path = context.HttpContext.Request.Path;
            var method = context.HttpContext.Request.Method;
            var instance = context.ActionDescriptor.DisplayName;

            context.HttpContext.Response.StatusCode = statusCodeValue;

            if (context.Exception is HttpStatusException httpStatusException)
            {
                statusCodeValue = (int)httpStatusException.StatusCode;

                responseModel = ApiResponseModelFactory.Create(httpStatusException.StatusCode, message:
                    httpStatusException.Message,
                    data: httpStatusException.GetDetails());
            }

            if(context.Exception is SomethingWrongException somethingWrongException)
            {
                responseModel = ApiResponseModelFactory.Create(statusCode, somethingWrongException.Message, somethingWrongException.GetDetails());
            }

            if(context.Exception is ApiException apiException)
            {
                responseModel = ApiResponseModelFactory.Create(statusCode, apiException.Message, apiException.Error);
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

                responseModel = ApiResponseModelFactory.Create(statusCodeValue, aggregateException.Message, new ErrorModel
                {
                    Code = "HTTP 500",
                    Message = aggregateException.Message,
                    InnerErrors = innerErrors,
                });
            }

            if (responseModel == null )
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


                responseModel = ApiResponseModelFactory.Create(statusCodeValue, context.Exception.Message, new ErrorModel
                {
                    Code = "HTTP 500",
                    Message = context.Exception.Message,
                    InnerErrors = innerErrors,
                });
            }

            if (responseModel is ApiResponseModel temp)
            {
                temp.Path = path;
                temp.Instance = instance;
                temp.Method = method;
            }            

            actionResult = new ObjectResult(responseModel);

            context.HttpContext.Response.StatusCode = statusCodeValue;
            context.Result = actionResult;
        }
    }
}
