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
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Builder;

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
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiVersioningAndSwaggerGen<ActualConfigureSwaggerOptions>(
            this IServiceCollection services, 
            ApiVersion apiVersion = default, 
            Action<SwaggerGenOptions> setupAction = null) where ActualConfigureSwaggerOptions : ConfigureSwaggerOptionsBase
        {
            var actualApiVersion = apiVersion ?? new ApiVersion(1, 0);

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.RegisterMiddleware = true;
                options.DefaultApiVersion = actualApiVersion;
                options.ReportApiVersions = true;                
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.DefaultApiVersion = actualApiVersion;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ActualConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                if (setupAction != null)
                {
                    setupAction(options);
                }
            });

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
        public static IServiceCollection AddApiVersioningAndSwaggerGen(
            this IServiceCollection services, 
            ApiVersion defaultVersion = default, 
            Action<SwaggerGenOptions> setupAction = null)
        {
            services.AddApiVersioningAndSwaggerGen<DefaultSwaggerOptions>(defaultVersion, setupAction);

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
        public static IServiceCollection ConfigureAppOptions(this IServiceCollection services, IConfiguration configuration = null)
        {
            if (configuration == null)
            {
                services.AddOptions<AppOptions>().Configure<IConfiguration>((options, configuration) => configuration.GetSection(AppOptions.Name));
                services.AddOptions<OpenApiInfo>().Configure<IConfiguration>((options, configuration) => configuration.GetSection(AppOptions.Name));
            }
            else
            {
                services.Configure<AppOptions>(configuration.GetSection(AppOptions.Name));
                services.Configure<OpenApiInfo>(configuration.GetSection(AppOptions.Name));
            }

            return services;
        }

        public static IServiceCollection AddHealthCheck<THealthChecker>(this IServiceCollection services, string name= "default_health_check") where THealthChecker : HealthCheckBase
        {
            services.AddHealthChecks()
                .AddCheck<THealthChecker>(name);

            return services;
        }

        /// <summary>
        /// Use forwarded headers 
        /// </summary>
        /// <remarks>https://docs.microsoft.com/ko-kr/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0#forwarded-headers</remarks>
        /// <param name="services"></param>
        /// <param name="enabledForwardedHeaders">Typically use this code `Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED")`</param>
        /// <param name="configureMoreOptions">Configure more</param>
        /// <returns></returns>
        public static IServiceCollection AddForwardedHeaders(this IServiceCollection services, 
            bool enabledForwardedHeaders = false, 
            Action<ForwardedHeadersOptions> configureMoreOptions = null)
        {
            if (enabledForwardedHeaders)
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                        ForwardedHeaders.XForwardedProto;
                    // Only loopback proxies are allowed by default.
                    // Clear that restriction because forwarders are enabled by explicit 
                    // configuration.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                    
                    if (configureMoreOptions != null)
                    {
                        configureMoreOptions(options);
                    }
                });
            }

            return services;
        }
    }
}
