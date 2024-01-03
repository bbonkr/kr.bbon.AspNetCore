using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NET8_0_OR_GREATER
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
#else
using Microsoft.AspNetCore.Mvc.ApiExplorer;
#endif
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace kr.bbon.AspNetCore.Options
{
    public abstract class ConfigureSwaggerOptionsBase : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptionsBase(IApiVersionDescriptionProvider provider) =>
          this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            ConfigureSwaggerOptions(options);
        }

        public abstract string AppTitle { get; }

        public abstract string AppDescription { get; }

        private void ConfigureSwaggerOptions(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                CustomOperationIds(options, description);
                SwaggerDoc(options, description);
            }
        }

        /// <summary>
        /// Configure SwaggerDoc
        /// </summary>
        /// <param name="options"></param>
        /// <param name="apiVersionDescription"></param>
        protected abstract void SwaggerDoc(SwaggerGenOptions options, ApiVersionDescription apiVersionDescription);

        /// <summary>
        /// Configure Custom operation ids
        /// </summary>
        /// <param name="options"></param>
        /// <param name="apiVersionDescription"></param>
        protected abstract void CustomOperationIds(SwaggerGenOptions options, ApiVersionDescription apiVersionDescription);

        protected string GetDelimiter(string value, string delimiter = "_")
        {
            if (!string.IsNullOrEmpty(value))
            {
                return "";
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// Default Swagger Options
    /// <para>
    /// This is deprecated, and might schedule to remove next version. Use <see cref="DefaultSwaggerOptions"/> instead.
    /// </para>
    /// 
    /// </summary>

    [Obsolete("Use DefaultSwaggerOptions instead.")]
    public class ConfigureSwaggerOptions : DefaultSwaggerOptions
    {
        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            IOptionsMonitor<AppOptions> appOptionsAccessor) : base(provider, appOptionsAccessor, null)
        {
            appOptions = appOptionsAccessor.CurrentValue ?? new AppOptions
            {
                Title = "Api",
                Description = "",
            };
        }

        public override string AppTitle => appOptions.Title;

        public override string AppDescription => appOptions.Description;

        private readonly AppOptions appOptions;
    }

    /// <summary>
    /// Default Swagger options
    /// </summary>
    public class DefaultSwaggerOptions : ConfigureSwaggerOptionsBase
    {
        public DefaultSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            IOptionsMonitor<AppOptions> appOptionsAccessor,
            IOptionsMonitor<OpenApiInfo> openApiInfoAccessor) : base(provider)
        {
            appOptions = appOptionsAccessor.CurrentValue ?? new AppOptions
            {
                Title = "Api",
                Description = "",
            };
            openApiInfo = openApiInfoAccessor.CurrentValue ?? new OpenApiInfo
            {
                Title = appOptions.Title,
                Description = appOptions.Description,
            };
        }

        protected override void SwaggerDoc(SwaggerGenOptions options, ApiVersionDescription apiVersionDescription)
        {
            if (string.IsNullOrWhiteSpace(openApiInfo.Version))
            {
                openApiInfo.Version = apiVersionDescription.ApiVersion.ToString();
            }

            options.SwaggerDoc(apiVersionDescription.GroupName, openApiInfo);
        }

        /// <summary>
        /// id template: {area:api}{version:v1.1.0-alpha}{controller:Users}{action:GetUsers}
        /// </summary>
        /// <param name="options"></param>
        /// <param name="apiVersionDescription"></param>
        protected override void CustomOperationIds(SwaggerGenOptions options, ApiVersionDescription apiVersionDescription)
        {
            options.CustomOperationIds(d =>
            {
                var area = d.ActionDescriptor.RouteValues["area"] ?? string.Empty;
                // https://github.com/microsoft/aspnet-api-versioning/blob/master/src/Common/Versioning/ApiVersionFormatProvider.cs
                var version = d.GetApiVersion().ToString("'v'VVVV");

                var controller = d.ActionDescriptor.RouteValues["controller"];
                var action = d.ActionDescriptor.RouteValues["action"] ?? d.HttpMethod;

                return $"{area}{GetDelimiter(area, string.Empty)}{version}{GetDelimiter(version, string.Empty)}{controller}{GetDelimiter(controller)}{action}";
            });
        }

        public override string AppTitle => appOptions.Title;

        public override string AppDescription => appOptions.Description;

        private readonly AppOptions appOptions;
        private readonly OpenApiInfo openApiInfo;
    }


}
