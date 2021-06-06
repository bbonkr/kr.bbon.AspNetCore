using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kr.bbon.AspNetCore.Options;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;
using kr.bbon.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Configre api versioning and swagger generator
        /// <para>
        /// If you want to set app title and description, add <see cref="ConfigureAppOptions"/> before this.
        /// </para>
        /// </summary>
        /// <typeparam name="ActualConfigureSwaggerOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="apiVersion">If default is not set, use 1.0</param>
        /// <returns></returns>
        public static IServiceCollection AddApiVersioningAndSwaggerGen<ActualConfigureSwaggerOptions>(this IServiceCollection services, ApiVersion apiVersion = default) where ActualConfigureSwaggerOptions : ConfigureSwaggerOptionsBase
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = apiVersion;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.DefaultApiVersion = apiVersion;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ActualConfigureSwaggerOptions>();

            services.AddSwaggerGen();

            return services;
        }

        /// <summary>
        /// Configre api versioning and swagger generator with <see cref="DefaultSwaggerOptions"/>.
        /// <para>
        /// If you want to set app title and description, add <see cref="ConfigureAppOptions"/> before this.
        /// </para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="defaultVersion"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiVersioningAndSwaggerGen(this IServiceCollection services, ApiVersion defaultVersion = default)
        {
            services.AddApiVersioningAndSwaggerGen<DefaultSwaggerOptions>(defaultVersion);

            return services;
        }


        /// <summary>
        /// Configure App options.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Need to Add 'App' property in appsettings.json
        /// <code>
        /// {
        ///     "App": {
        ///         "Title": "app title here",
        ///         "Description": "app description here"
        ///     }
        /// }
        /// </code>
        /// </para>
        /// </remarks>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// {
        ///     "App": {
        ///         "Title": "app title here",
        ///         "Description": "app description here"
        ///     }
        /// }
        /// </code>
        /// </example>
        public static IServiceCollection ConfigureAppOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppOptions>(configuration.GetSection(AppOptions.Name));

            return services;
        }
    }
}
