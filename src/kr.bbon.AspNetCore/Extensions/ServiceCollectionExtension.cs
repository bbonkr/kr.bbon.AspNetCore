using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kr.bbon.AspNetCore.Options;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace kr.bbon.AspNetCore
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Configre api versioning and swagger generator
        /// </summary>
        /// <typeparam name="ActualConfigureSwaggerOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="defaultVersion">If default is not set, use 1.0</param>
        /// <returns></returns>
        public static IServiceCollection AddApiVersioningAndSwaggerGen<ActualConfigureSwaggerOptions>(this IServiceCollection services, ApiVersion defaultVersion = default) where ActualConfigureSwaggerOptions : ConfigureSwaggerOptionsBase
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = defaultVersion;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.DefaultApiVersion = defaultVersion;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ActualConfigureSwaggerOptions>();

            services.AddSwaggerGen();

            return services;
        }

        public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, IConfigurationSection configurationSection) where TOptions : class
        {
            services.Configure<TOptions>(configurationSection);

            return services;
        }
    }
}
