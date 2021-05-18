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
        public override ObjectResult StatusCode([ActionResultStatusCode] int statusCode, [ActionResultObjectValue] object value)
        {
            return base.StatusCode(statusCode, ApiResponseModelFactory.Create(statusCode, string.Empty, value));
        }

        protected ObjectResult StatusCode<T>(HttpStatusCode statusCode, string message, T value) where T:class, new()
        {
            return base.StatusCode((int)statusCode, ApiResponseModelFactory.Create<T>(statusCode, message, value));
        }

        protected ObjectResult StatusCode<T>(HttpStatusCode statusCode, T value) where T : class, new()
        {
            return base.StatusCode((int)statusCode, ApiResponseModelFactory.Create(statusCode, string.Empty, value));
        }

        protected ObjectResult StatusCode(HttpStatusCode statusCode, string message)
        {
            return base.StatusCode((int)statusCode, ApiResponseModelFactory.Create(statusCode, message));
        }
    }
}
