using System;

namespace kr.bbon.AspNetCore
{
    /// <summary>
    /// Represent 'App' at appsettings.json
    /// </summary>
    /// <example>appsettings.json
    /// <code>
    /// {
    ///     "App": {
    ///         "Title": "Your application title",
    ///         "Description": "Description of your application"
    ///     }
    /// }
    /// </code>
    /// </example>
    public class AppOptions
    {
        public static string Name = "App";

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
