using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kr.bbon.Core.Models;
using kr.bbon.Core;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace kr.bbon.AspNetCore.Extensions.DependencyInjection
{
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Configure ApiBehaviorOptions 
        /// <para>
        /// Default options:
        /// <list type="number">
        ///     <item>Throws ApiException when model is invalid.</item>
        /// </list>
        /// 
        /// </para>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public static IMvcBuilder ConfigureDefaultApiBehaviorOptions(this IMvcBuilder builder, Action<ApiBehaviorOptions> setupAction = null)
        {
            var defaultApiBehaviorOptionsSetupAction = new Action<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new ErrorModel(e.Value.Errors.First().ErrorMessage, e.Key));

                    throw new ApiException(
                        StatusCodes.Status400BadRequest,
                        new ErrorModel(
                            Message: errors.FirstOrDefault().Message,
                            InnerErrors: errors)
                        );
                };
            });

            builder.ConfigureApiBehaviorOptions(setupAction ?? defaultApiBehaviorOptionsSetupAction);

            return builder;
        }
    }
}
