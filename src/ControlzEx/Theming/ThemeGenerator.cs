﻿#nullable enable
namespace ControlzEx.Theming
{
    using System.Collections.Generic;
    using System.Diagnostics;

    // This class has to be kept in sync with https://github.com/batzen/XamlColorSchemeGenerator/blob/develop/src/ColorSchemeGenerator.cs
    // Please do not remove unused code/properties here as it makes syncing more difficult.
    public static class ThemeGenerator
    {
        public static ThemeGeneratorParameters GetParametersFromString(string input)
        {
#if NETCOREAPP
            return System.Text.Json.JsonSerializer.Deserialize<ThemeGeneratorParameters>(input);
#else
            return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ThemeGeneratorParameters>(input);
#endif
        }

        // The order of the passed valueSources is important.
        // More specialized/concrete values must be passed first and more generic ones must follow.
        public static string GenerateColorSchemeFileContent(string templateContent, string themeName, string themeDisplayName, string baseColorScheme, string colorScheme, string alternativeColorScheme, params Dictionary<string, string>[] valueSources)
        {
            templateContent = templateContent.Replace("{{ThemeName}}", themeName);
            templateContent = templateContent.Replace("{{ThemeDisplayName}}", themeDisplayName);
            templateContent = templateContent.Replace("{{BaseColorScheme}}", baseColorScheme);
            templateContent = templateContent.Replace("{{ColorScheme}}", colorScheme);
            templateContent = templateContent.Replace("{{AlternativeColorScheme}}", alternativeColorScheme);

            foreach (var valueSource in valueSources)
            {
                foreach (var value in valueSource)
                {
                    templateContent = templateContent.Replace($"{{{{{value.Key}}}}}", value.Value);   
                }
            }

            return templateContent;
        }

        public class ThemeGeneratorParameters
        {
            public Dictionary<string, string> DefaultValues { get; set; } = new Dictionary<string, string>();

            public ThemeGeneratorBaseColorScheme[] BaseColorSchemes { get; set; } = new ThemeGeneratorBaseColorScheme[0];

            public ThemeGeneratorColorScheme[] ColorSchemes { get; set; } = new ThemeGeneratorColorScheme[0];

            public AdditionalColorSchemeVariant[] AdditionalColorSchemeVariants { get; set; } = new AdditionalColorSchemeVariant[0];
        }

        [DebuggerDisplay("{" + nameof(Name) + "}")]
        public class ThemeGeneratorBaseColorScheme
        {
            public string Name { get; set; } = string.Empty;

            public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
        }

        [DebuggerDisplay("{" + nameof(Name) + "}")]
        public class AdditionalColorSchemeVariant
        {
            public string Name { get; set; } = string.Empty;

            public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
        }

        [DebuggerDisplay("{" + nameof(Name) + "}")]
        public class ThemeGeneratorColorScheme
        {
            public string Name { get; set; } = string.Empty;

            public string ColorSchemeVariantName { get; set; } = string.Empty;

            public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
        }
    }
}