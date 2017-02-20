﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Net.Mime;
    using System.Windows;
    using System.Windows.Media;
    using Windows;
    using Catel.IoC;
    using Fluent;
    using Services;

    /// <summary>
    /// Interaction logic for ShellWindow.xaml.
    /// </summary>
    public partial class ShellWindow : IShell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellWindow"/> class.
        /// </summary>
        public ShellWindow()
        {
            var serviceLocator = ServiceLocator.Default;

            var themeService = serviceLocator.ResolveType<IThemeService>();
            ThemeHelper.EnsureApplicationThemes(GetType().Assembly, themeService.ShouldCreateStyleForwarders());

            // Required for Fluent accent color
            var accentColor = ThemeHelper.GetAccentColor();
            var applicationResources = Application.Current.Resources;
            applicationResources.Add(MetroColors.ThemeColorKey, accentColor);

            InitializeComponent();

            serviceLocator.RegisterInstance(pleaseWaitProgressBar, "pleaseWaitService");

            var statusService = serviceLocator.ResolveType<IStatusService>();
            statusService.Initialize(statusTextBlock);

            var dependencyResolver = this.GetDependencyResolver();
            var ribbonService = dependencyResolver.Resolve<IRibbonService>();

            var ribbonContent = ribbonService.GetRibbon();
            if (ribbonContent != null)
            {
                ribbonContentControl.SetCurrentValue(ContentProperty, ribbonContent);
            }

            var statusBarContent = ribbonService.GetStatusBar();
            if (statusBarContent != null)
            {
                customStatusBarItem.SetCurrentValue(ContentProperty, statusBarContent);
            }

            contentControl.Content = ribbonService.GetMainView();
        }
    }
}