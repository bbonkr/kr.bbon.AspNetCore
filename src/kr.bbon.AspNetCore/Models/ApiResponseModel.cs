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

        public string Instance { get; set; }

        public string Path { get; set; }

        public string Method { get; set; }
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
        public static ApiResponseModel<object> Create(HttpStatusCode statusCode, string message)
        {
            return Create<object>(
                statusCode: (int)statusCode,
                message: message,
                instance: default,
                data: default);
        }

        public static ApiResponseModel<object> Create(HttpStatusCode statusCode)
        {
            return Create<object>(
                 statusCode: (int)statusCode,
                 message: default,
                 instance: default,
                 data: default);
        }

        public static ApiResponseModel<T> Create<T>(HttpStatusCode statusCode, T data)
        {
            return Create(
                statusCode: (int)statusCode,
                message: default,
                instance: default,
                data: data);
        }

        public static ApiResponseModel<T> Create<T>(HttpStatusCode statusCode, string message, T data)
        {
            return Create(
                statusCode: (int)statusCode,
                message: message,
                instance: default,
                data: data);
        }

        public static ApiResponseModel<T> Create<T>(int statusCode, string message, T data)
        {
            return Create(
                statusCode: statusCode,
                message: message,
                instance: default,
                data: data);
        }

        public static ApiResponseModel<T> Create<T>(int statusCode, string message = default, string instance = default, T data = default)
        {
            return new ApiResponseModel<T>
            {
                StatusCode = statusCode,
                Message = message ?? $"{(HttpStatusCode)statusCode}",
                Data = data,
                Instance = instance,
            };
        }
    }
}
