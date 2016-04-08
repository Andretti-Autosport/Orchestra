﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Linq;
    using System.Windows;
    using Catel.Logging;
    using MahApps.Metro;

    public static class MahAppsHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Sets the theme color of the application. This method dynamically creates an in-memory resource
        /// dictionary containing the accent colors used by MahApps.
        /// </summary>
        public static void ApplyTheme()
        {
            //<Color x:Key="HighlightColor">
            //    #800080
            //</Color>
            //<Color x:Key="AccentColor">
            //    #CC800080
            //</Color>
            //<Color x:Key="AccentColor2">
            //    #99800080
            //</Color>
            //<Color x:Key="AccentColor3">
            //    #66800080
            //</Color>
            //<Color x:Key="AccentColor4">
            //    #33800080
            //</Color>

            //<SolidColorBrush x:Key="HighlightBrush" Color="{StaticResource HighlightColor}" />
            //<SolidColorBrush x:Key="AccentColorBrush" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="AccentColorBrush2" Color="{StaticResource AccentColor2}" />
            //<SolidColorBrush x:Key="AccentColorBrush3" Color="{StaticResource AccentColor3}" />
            //<SolidColorBrush x:Key="AccentColorBrush4" Color="{StaticResource AccentColor4}" />
            //<SolidColorBrush x:Key="WindowTitleColorBrush" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="AccentSelectedColorBrush" Color="White" />
            //<LinearGradientBrush x:Key="ProgressBrush" EndPoint="0.001,0.5" StartPoint="1.002,0.5">
            //    <GradientStop Color="{StaticResource HighlightColor}" Offset="0" />
            //    <GradientStop Color="{StaticResource AccentColor3}" Offset="1" />
            //</LinearGradientBrush>
            //<SolidColorBrush x:Key="CheckmarkFill" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="RightArrowFill" Color="{StaticResource AccentColor}" />

            //<Color x:Key="IdealForegroundColor">
            //    Black
            //</Color>
            //<SolidColorBrush x:Key="IdealForegroundColorBrush" Color="{StaticResource IdealForegroundColor}" />

            // Theme is always the 0-index of the resources
            var application = Application.Current;
            var applicationResources = application.Resources;
            var resourceDictionary = ThemeHelper.GetAccentColorResourceDictionary();

            var applicationTheme = ThemeManager.AppThemes.First(x => string.Equals(x.Name, "BaseLight"));

            // Insert to get the best MahApps performance (when looking up themes)
            //applicationResources.MergedDictionaries.Remove(resourceDictionary);
            //applicationResources.MergedDictionaries.Insert(1, resourceDictionary);
            applicationResources.MergedDictionaries.Insert(2, applicationTheme.Resources);

            Log.Debug("Applying theme to MahApps");

            var theme = ThemeManager.DetectAppStyle(application);

            //var newAccent = new Accent
            //{
            //    Name = "Runtime accent (Orchestra)",
            //    Resources = resourceDictionary
            //};

            //// Workaround for bug in MahApps.Metro: https://github.com/MahApps/MahApps.Metro/issues/2300
            //// Temporarily remove the resource dictionary so we can safely call ChangeAppStyle. Once this is
            //// fixed, we only have to call ThemeManager.ChangeAppStyle

            //ThemeManager.ChangeAppStyle(application, newAccent, applicationTheme);

            //applicationResources.MergedDictionaries.Insert(0, resourceDictionary);
        }
    }
}