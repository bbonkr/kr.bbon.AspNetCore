using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace kr.bbon.AspNetCore.Models
{
    public interface IApiResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string Instance { get; set; }

        public string Path { get; set; }

        public string Method { get; set; }
    }

    public interface IApiResponseWithData<T> : IApiResponse
    {
        public T Data { get; set; }
    }

    public class ApiResponseModel<T> : IApiResponseWithData<T>
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string Instance { get; set; }

        public string Path { get; set; }

        public string Method { get; set; }

        public T Data { get; set; }
    }

    /// <summary>
    /// Factory for <see cref="ApiResponseModel"/>
    /// </summary>
    public static class ApiResponseModelFactory
    {
        public static ApiResponseModel<object> Create(HttpStatusCode statusCode, string message)
        {
            return Create(
                statusCode: (int)statusCode,
                message: message,
                instance: default,
                data: default(object));
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
