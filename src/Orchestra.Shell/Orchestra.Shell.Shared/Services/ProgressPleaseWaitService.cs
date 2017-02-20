﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressPleaseWaitService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;

    internal class ProgressPleaseWaitService : PleaseWaitService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDependencyResolver _dependencyResolver;
        private ProgressBar _progressBar;
        private ResourceDictionary _resourceDictionary;

        public ProgressPleaseWaitService(IDispatcherService dispatcherService, IDependencyResolver dependencyResolver)
            : base(dispatcherService)
        {
            Argument.IsNotNull(() => dependencyResolver);

            _dependencyResolver = dependencyResolver;
        }

        public override void Hide()
        {
            base.Hide();

            var progressBar = InitializeProgressBar();
            if (progressBar != null)
            {
                _dispatcherService.BeginInvoke(() =>
                {
                    Log.Debug("Hiding progress bar");

                    progressBar.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Hidden);
                });
            }
        }

        public override void UpdateStatus(int currentItem, int totalItems, string statusFormat = "")
        {
            base.UpdateStatus(currentItem, totalItems, statusFormat);

            var progressBar = InitializeProgressBar();
            if (progressBar != null)
            {
                _dispatcherService.BeginInvoke(() =>
                {
                    progressBar.SetCurrentValue(System.Windows.Controls.Primitives.RangeBase.MinimumProperty, (double)0);
                    progressBar.SetCurrentValue(System.Windows.Controls.Primitives.RangeBase.MaximumProperty, (double)totalItems);
                    progressBar.SetCurrentValue(System.Windows.Controls.Primitives.RangeBase.ValueProperty, (double)currentItem);

                    if (currentItem < 0 || currentItem >= totalItems)
                    {
                        Log.Debug("Hiding progress bar");

                        var storyboard = GetHideProgressBarStoryboard();
                        storyboard.Completed += (sender, e) =>
                        {
                            progressBar.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Hidden);
                        };

                        storyboard.Begin(progressBar);
                    }
                    else if (progressBar.Visibility != Visibility.Visible)
                    {
                        Log.Debug("Showing progress bar");

                        progressBar.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Visible);
                    }
                });
            }
        }

        private ProgressBar InitializeProgressBar()
        {
            if (_progressBar == null)
            {
                _progressBar = _dependencyResolver.TryResolve<ProgressBar>("pleaseWaitService");

                if (_progressBar != null)
                {
                    Log.Debug("Found progress bar that will represent progress inside the ProgressPleaseWaitService");
                }
            }

            return _progressBar;
        }

        private Storyboard GetHideProgressBarStoryboard()
        {
            if (_resourceDictionary == null)
            {
                _resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("/Orchestra.Core;Component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
                };
            }

            var storyBoard = (Storyboard)_resourceDictionary["FadeOutStoryboard"];
            return storyBoard;
        }
    }
}