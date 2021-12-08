using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace kr.bbon.AspNetCore.DependencyInjection
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseSwaggerUIWithApiVersioning(this IApplicationBuilder app, Action<SwaggerUIOptions> setupAction = null)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions.Reverse())
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                if (setupAction != null)
                {
                    setupAction(options);
                }
            });

            return app;
        }
    }
}
