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

        protected virtual void HandleException(ExceptionContext context)
        {
            ObjectResult actionResult = default;

            IApiResponse responseModel = default;

            var statusCode = HttpStatusCode.InternalServerError;
            var statusCodeValue = (int)statusCode;

            var path = context.HttpContext.Request.Path;
            var method = context.HttpContext.Request.Method;
            var instance = context.ActionDescriptor.DisplayName;            

            if (context.Exception is HttpStatusException httpStatusException)
            {
                statusCodeValue = (int)httpStatusException.StatusCode;

                responseModel = ApiResponseModelFactory.Create(statusCodeValue, message:
                    httpStatusException.Message,
                    data: httpStatusException.GetDetails());
            }

            if (context.Exception is SomethingWrongException somethingWrongException)
            {
                responseModel = ApiResponseModelFactory.Create(statusCodeValue, somethingWrongException.Message, somethingWrongException.GetDetails());
            }

            if (context.Exception is ApiException apiException)
            {
                statusCodeValue = apiException.StatusCode;
                responseModel = ApiResponseModelFactory.Create(statusCodeValue, apiException.Message, apiException.Error);
            }

            if (context.Exception is AggregateException aggregateException)
            {
                var innerErrors = aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count > 0
                        ? aggregateException.InnerExceptions.Select((inner, index) => new ErrorModel(inner.Message, Code: $"Inner Error {index + 1}")).ToList()
                        : new List<ErrorModel>();

                responseModel = ApiResponseModelFactory.Create(
                    statusCodeValue,
                    aggregateException.Message,
                    new ErrorModel(aggregateException.Message, Code: $"{(HttpStatusCode)statusCodeValue}", InnerErrors: innerErrors));
            }

            if (responseModel == null)
            {
                var innerErrors = context.Exception.InnerException != null
                    ? new List<ErrorModel>{
                        new ErrorModel(context.Exception.InnerException.Message)
                    }
                    : new List<ErrorModel>();


                responseModel = ApiResponseModelFactory.Create(statusCodeValue, context.Exception.Message, new ErrorModel(context.Exception.Message, Code: $"{(HttpStatusCode)statusCodeValue}", InnerErrors: innerErrors));
            }

            responseModel.Path = path;
            responseModel.Instance = instance;
            responseModel.Method = method;

            actionResult = new ObjectResult(responseModel);

            context.HttpContext.Response.StatusCode = statusCodeValue;
            context.Result = actionResult;
        }
    }
}
