/**
 * extensions of Object type 
 */
namespace kr.bbon.AspNetCore.Extensions.DependencyInjection
{
    using System;
    using System.Text.Encodings.Web;
    using System.Text.Json;

    public static class ObjectExtensions
    {
        /// <summary>
        /// Serialize object to json
        /// </summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="obj">Target object</param>
        /// <param name="options">Json serializer options</param>
        /// <returns>json string</returns>
        public static string ToJson<T>(this T obj, JsonSerializerOptions options = null)
        {
            if(obj == null)
            {
                return string.Empty;
            }

            var actualOptions = options ?? new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            return JsonSerializer.Serialize<T>(obj, actualOptions);
        }
    }
}
