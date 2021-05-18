using System;
using System.Net;


namespace kr.bbon.AspNetCore
{
    /// <summary>
    /// Exception with http status code and message
    /// </summary>
    public abstract class HttpStatusException : Exception
    {
        public HttpStatusException(HttpStatusCode httpStatusCode, string message)
            : base(message)
        {
            this.StatusCode = httpStatusCode;
        }
        public HttpStatusException(int httpStatusCode, string message)
            : this((HttpStatusCode)httpStatusCode, message) { }

        public HttpStatusCode StatusCode { get; init; }

        public abstract object GetDetails();

        public abstract T GetDetails<T>() where T : class;
    }

    /// <summary>
    /// Exception with http status code, message, detail information
    /// </summary>
    public class HttpStatusException<TDetails> : HttpStatusException where TDetails : class
    {
        public HttpStatusException(HttpStatusCode httpStatusCode, string message, TDetails details)
            : base(httpStatusCode, message)
        {
            this.Details = details;
        }

        public HttpStatusException(int httpStatusCode, string message, TDetails details)
            : this((HttpStatusCode)httpStatusCode, message, details) { }

        public TDetails Details { get; init; }

        public override object GetDetails()
        {
            return Details;
        }

        public override T GetDetails<T>() where T : class
        {
            return (T)GetDetails();
        }
    }
}
