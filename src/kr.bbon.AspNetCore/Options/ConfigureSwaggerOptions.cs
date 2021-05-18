using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                  description.GroupName,
                    new OpenApiInfo()
                    {
                        Title = $"{AppTitle} {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = AppDescription,
                    });
            }
        }

        public abstract string AppTitle { get; }

        public abstract string AppDescription { get; }
    }

    /// <summary>
    /// Default Swagger Options
    /// <para>
    /// This is deprecated, and might schedule to remove next version. Use <see cref="DefaultSwaggerOptions"/> instead.
    /// </para>
    /// 
    /// </summary>

    [Obsolete("Use DefaultSwaggerOptions instead.")]
    public class ConfigureSwaggerOptions : ConfigureSwaggerOptionsBase
    {
        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            IOptionsMonitor<AppOptions> appOptionsAccessor) : base(provider)
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
            IOptionsMonitor<AppOptions> appOptionsAccessor) : base(provider)
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
}
