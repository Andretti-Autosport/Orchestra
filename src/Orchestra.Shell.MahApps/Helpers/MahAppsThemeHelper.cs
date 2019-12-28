﻿// Note: this code comes from the MahApps.Metro showcase example licensed MIT:
// https://github.com/MahApps/MahApps.Metro/blob/develop/src/Mahapps.Metro.Tests/TestHelpers/ThemeHelper.cs

namespace Orchestra
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;
    using MahApps.Metro;
    using Newtonsoft.Json;
    using XamlColorSchemeGenerator;
    using ColorScheme = XamlColorSchemeGenerator.ColorScheme;

    public static class MahAppsThemeHelper
    {
        public static string GenerateThemeName(string baseColorScheme, Color accentBaseColor, Color highlightColor)
        {
            return $"RuntimeTheme_{baseColorScheme}_{accentBaseColor.ToString().Replace("#", string.Empty)}";
        }

        public static ResourceDictionary CreateTheme(string baseColorScheme, Color accentBaseColor, string name = null, bool changeImmediately = false)
        {
            name = name ?? GenerateThemeName(baseColorScheme, accentBaseColor, accentBaseColor);

            var generatorParameters = GetGeneratorParameters();
            var themeTemplateContent = GetThemeTemplateContent();

            var variant = generatorParameters.BaseColorSchemes.First(x => x.Name == baseColorScheme);
            var colorScheme = new ColorScheme();
            colorScheme.Name = accentBaseColor.ToString().Replace("#", string.Empty);
            var values = colorScheme.Values;
            values.Add("MahApps.Colors.AccentBase", accentBaseColor.ToString());
            values.Add("MahApps.Colors.Accent", Color.FromArgb(204, accentBaseColor.R, accentBaseColor.G, accentBaseColor.B).ToString());
            values.Add("MahApps.Colors.Accent2", Color.FromArgb(153, accentBaseColor.R, accentBaseColor.G, accentBaseColor.B).ToString());
            values.Add("MahApps.Colors.Accent3", Color.FromArgb(102, accentBaseColor.R, accentBaseColor.G, accentBaseColor.B).ToString());
            values.Add("MahApps.Colors.Accent4", Color.FromArgb(51, accentBaseColor.R, accentBaseColor.G, accentBaseColor.B).ToString());

            values.Add("MahApps.Colors.Highlight", accentBaseColor.ToString());
            values.Add("MahApps.Colors.IdealForeground", IdealTextColor(accentBaseColor).ToString());

            var xamlContent = new ColorSchemeGenerator().GenerateColorSchemeFileContent(generatorParameters, variant, colorScheme, themeTemplateContent, name, name);

            var resourceDictionary = (ResourceDictionary)XamlReader.Parse(xamlContent);

            var newTheme = new Theme(resourceDictionary);

            ThemeManager.AddTheme(newTheme.Resources);

            // Apply theme
            if (changeImmediately)
            {
                ThemeManager.ChangeTheme(Application.Current, newTheme);
            }

            return resourceDictionary;
        }

        public static string GetResourceDictionaryContent(ResourceDictionary resourceDictionary)
        {
            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "    "
                }))
                {
                    XamlWriter.Save(resourceDictionary, writer);

                    return sw.ToString();
                }
            }
        }

        /// <summary>
        ///     Determining Ideal Text Color Based on Specified Background Color
        ///     http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name="color">The bg.</param>
        /// <returns></returns>
        private static Color IdealTextColor(Color color)
        {
            const int nThreshold = 105;
            var bgDelta = Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            var foreColor = 255 - bgDelta < nThreshold
                                ? Colors.Black
                                : Colors.White;
            return foreColor;
        }

        private static string GetThemeTemplateContent()
        {
            using (var stream = typeof(ThemeManager).Assembly.GetManifestResourceStream("MahApps.Metro.Styles.Themes.Theme.Template.xaml"))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static GeneratorParameters GetGeneratorParameters()
        {
            return JsonConvert.DeserializeObject<GeneratorParameters>(GetGeneratorParametersJson());
        }

        private static string GetGeneratorParametersJson()
        {
            using (var stream = typeof(ThemeManager).Assembly.GetManifestResourceStream("MahApps.Metro.Styles.Themes.GeneratorParameters.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
