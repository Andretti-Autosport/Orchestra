﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Windows
{
    using System;
    using System.Windows;
    using Catel;

    /// <summary>
    /// Window extensions class.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Applies the application icon to the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="window"/> is <c>null</c>.</exception>
        public static void ApplyApplicationIcon(this Window window)
        {
            Argument.IsNotNull(() => window);

            var application = Application.Current;
            if (application != null)
            {
                var mainWindow = application.MainWindow;
                if (mainWindow != null)
                {
                    window.SetCurrentValue(Window.IconProperty, application.MainWindow.Icon);
                }
            }
        }
    }
}