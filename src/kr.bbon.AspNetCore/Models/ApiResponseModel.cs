using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace kr.bbon.AspNetCore.Models
{
    public abstract class ApiResponseModelBase
    {
        public int StatusCode { get; init; }

        public string Message { get; init; }
    }

    public class ApiResponseModel : ApiResponseModelBase
    {

    }

    public class ApiResponseModel<T> : ApiResponseModelBase 
    {
        public T Data { get; init; }
    }

    /// <summary>
    /// Factory for <see cref="ApiResponseModel"/>
    /// </summary>
    public class ApiResponseModelFactory
    {
        public static ApiResponseModel<object> Create(HttpStatusCode statusCode, string message = "")
        {
            return Create<object>((int)statusCode, message, default);
        }

        public static ApiResponseModel<object> Create(HttpStatusCode statusCode)
        {
            return Create<object>((int)statusCode, string.Empty, default);
        }

        public static ApiResponseModel<T> Create<T>(HttpStatusCode statusCode, T data = default) 
        {
            return Create((int)statusCode, string.Empty, data);
        }

        public static ApiResponseModel<T> Create<T>(HttpStatusCode statusCode, string message = "", T data = default) 
        {
            return Create((int)statusCode, message, data);
        }

        public static ApiResponseModel<T> Create<T>(int statusCode, string message = "", T data = default) 
        {
            return new ApiResponseModel<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data,
            };
        }      
    }
}
