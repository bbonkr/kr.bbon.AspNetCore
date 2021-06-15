using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using kr.bbon.AspNetCore.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace kr.bbon.AspNetCore.Mvc
{
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Creates a <see cref="ObjectResult"/> object by specifying a statusCode and value
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override ObjectResult StatusCode([ActionResultStatusCode] int statusCode, [ActionResultObjectValue] object value)
        {
            return base.StatusCode(statusCode, ApiResponseModelFactory.Create(statusCode, string.Empty, value));
        }

        /// <summary>
        /// Creates a <see cref="ObjectResult"/>  object by specifying a statusCode and value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ObjectResult StatusCode<T>(HttpStatusCode statusCode, string message, T value) 
        {
            return base.StatusCode((int)statusCode, ApiResponseModelFactory.Create(statusCode, message, value));
        }

        /// <summary>
        /// Creates a <see cref="ObjectResult"/> object by specifying a statusCode and value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statusCode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ObjectResult StatusCode<T>(HttpStatusCode statusCode, T value) 
        {
            return base.StatusCode((int)statusCode, ApiResponseModelFactory.Create(statusCode, string.Empty, value));
        }

        /// <summary>
        /// Creates a <see cref="ObjectResult"/> object by specifying a statusCode and value
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected ObjectResult StatusCode(HttpStatusCode statusCode, string message)
        {
            return base.StatusCode((int)statusCode, ApiResponseModelFactory.Create(statusCode, message));
        }

        /// <summary>
        /// Creates a <see cref="ObjectResult"/> object by specifying a statusCode and value
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        protected ObjectResult StatusCode(HttpStatusCode statusCode)
        {
            return base.StatusCode((int)statusCode, ApiResponseModelFactory.Create(statusCode));
        }

        /// <summary>
        /// Creates a <see cref="ObjectResult"/> object by specifying a statusCode and value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ObjectResult StatusCode<T>([ActionResultStatusCode] int statusCode, string message, T value)
        {
            return base.StatusCode(statusCode, ApiResponseModelFactory.Create(statusCode, message, value));
        }

        /// <summary>
        /// Creates a <see cref="ObjectResult"/> object by specifying a statusCode and value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statusCode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ObjectResult StatusCode<T>([ActionResultStatusCode] int statusCode, T value)
        {
            return base.StatusCode(statusCode, ApiResponseModelFactory.Create(statusCode, string.Empty, value));
        }

        /// <summary>
        /// Creates a <see cref="ObjectResult"/> object by specifying a statusCode and value
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected ObjectResult StatusCode([ActionResultStatusCode] int statusCode, string message)
        {
            return base.StatusCode(statusCode, ApiResponseModelFactory.Create<object>(statusCode, message, null));
        }

        /// <summary>
        /// Creates a <see cref="ObjectResult"/> object by specifying a statusCode and value
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        protected new ObjectResult StatusCode([ActionResultStatusCode] int statusCode)
        {
            return base.StatusCode(statusCode, ApiResponseModelFactory.Create<object>(statusCode, string.Empty, null));
        }
    }
}
