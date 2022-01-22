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
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace kr.bbon.AspNetCore.Extensions.DependencyInjection
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
            var serviceDescriptor = GetServiceDescriptor(services, typeof(OpenApiInfo));

            if (configuration == null)
            {
                services.AddOptions<AppOptions>().Configure<IConfiguration>((options, configuration) => configuration.GetSection(AppOptions.Name).Bind(options));

                if (serviceDescriptor == null)
                {
                    services.AddOptions<OpenApiInfo>().Configure<IConfiguration>((options, configuration) => configuration.GetSection(AppOptions.Name).Bind(options));
                }
            }
            else
            {
                services.Configure<AppOptions>(configuration.GetSection(AppOptions.Name));
                if (serviceDescriptor == null)
                {
                    services.Configure<OpenApiInfo>(configuration.GetSection(AppOptions.Name));
                }
            }

            return services;
        }

        /// <summary>
        /// Configure <see cref="OpenApiInfo"/>.
        /// <para>
        /// Fill complete this information to use.
        /// </para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureAction"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureOpenApiInfo(this IServiceCollection services, Action<OpenApiInfo> configureAction)
        {
            var serviceDescriptor = GetServiceDescriptor(services, typeof(OpenApiInfo));
            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }

            services.Configure<OpenApiInfo>(configureAction);

            return services;
        }

        private static ServiceDescriptor GetServiceDescriptor(IServiceCollection services, Type implementationType)
        {
            var serviceDescriptor = services.Where(x => x.ImplementationType == implementationType).FirstOrDefault();
            return serviceDescriptor;
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
            bool? enabledForwardedHeaders = null, 
            Action<ForwardedHeadersOptions> configureMoreOptions = null)
        {
            var enabledForwardedHeadersEnvironmentValue = Convert.ToBoolean(Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED"));
            var enabledForwardedHeadersValue = enabledForwardedHeaders.HasValue ? enabledForwardedHeaders.Value : enabledForwardedHeadersEnvironmentValue;

            if (enabledForwardedHeadersValue)
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

        /// <summary>
        /// Configure JsonSerializerOptions and JsonOptions
        /// <para>
        /// Settings:
        /// <list type="bullet">
        ///   <item>PropertyNamingPolicy: <see cref="JsonNamingPolicy.CamelCase"/></item>
        ///   <item>DefaultIgnoreCondition: <see cref="JsonIgnoreCondition.WhenWritingNull"/></item>
        ///   <item>DictionaryKeyPolicy: <see cref="JsonNamingPolicy.CamelCase"/></item>
        ///   <item>AllowTrailingCommas: true</item>
        ///   <item>IgnoreReadOnlyFields: true</item>
        ///   <item>IgnoreReadOnlyProperties: true</item>
        ///   <item>WriteIndented: true</item>
        ///   <item>NumberHandling: <see cref="JsonNumberHandling.AllowNamedFloatingPointLiterals"/></item>
        ///   <item>PropertyNameCaseInsensitive: true</item>
        ///   <item>Encoder: <see cref="System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/></item>
        ///   <item>
        ///   <para>
        ///     Converters:
        ///     <list type="bullet">
        ///       <item><see cref="JsonStringEnumConverter"/></item>
        ///     </list>
        ///   </para>
        ///   </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureAction"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDefaultJsonOptions(this IServiceCollection services, Action<JsonOptions> configureAction = null)
        {
            // TODO Try to configure JsonOptions using JsonSerializerOptions
            services.Configure<JsonSerializerOptions>(options =>
            {
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.AllowTrailingCommas = true;
                options.IgnoreReadOnlyFields = true;
                options.IgnoreReadOnlyProperties = true;
                options.WriteIndented = true;
                options.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.PropertyNameCaseInsensitive = true;
                // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-character-encoding#serialize-all-characters
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                options.Converters.Add(new JsonStringEnumConverter());
            });

            services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
                options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-character-encoding#serialize-all-characters
                options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                if (configureAction != null)
                {
                    configureAction(options);
                }
            });

            return services;
        }
    }
}
