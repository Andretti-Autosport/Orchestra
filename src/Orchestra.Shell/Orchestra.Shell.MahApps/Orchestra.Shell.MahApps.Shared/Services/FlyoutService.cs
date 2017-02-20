﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.Windows.Threading;
    using MahApps.Metro.Controls;
    using Models;
    using ThemeHelper = Orchestra.ThemeHelper;

    public class FlyoutService : IFlyoutService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICommandManager _commandManager;
        private readonly Dictionary<string, FlyoutInfo> _flyouts = new Dictionary<string, FlyoutInfo>();
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public FlyoutService(ITypeFactory typeFactory, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => commandManager);

            _typeFactory = typeFactory;
            _commandManager = commandManager;
        }
        #endregion

        #region Methods
        public IEnumerable<Flyout> GetFlyouts()
        {
            return (from flyout in _flyouts.Values
                select flyout.Flyout);
        }

        [ObsoleteEx(ReplacementTypeOrMember = "AddFlyout(string, Type, Position, UnloadBehavior, FlyoutTheme)", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        public void AddFlyout(string name, Type viewType, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel)
        {
            AddFlyout(name, viewType, position, unloadBehavior, FlyoutTheme.Adapt);
        }

        public void AddFlyout(string name, Type viewType, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel, FlyoutTheme flyoutTheme = FlyoutTheme.Adapt)
        {
            Argument.IsNotNullOrWhitespace(() => name);
            Argument.IsNotNull(() => viewType);

            Log.Info("Adding flyout '{0}' with view type '{1}'", name, viewType.FullName);

            var content = (UIElement)_typeFactory.CreateInstance(viewType);

            var flyout = new Flyout();
            flyout.Theme = flyoutTheme;
            flyout.Position = position;

            var flyoutInfo = new FlyoutInfo(flyout, content);

            flyout.SetBinding(Flyout.HeaderProperty, new Binding("ViewModel.Title") { Source = content });

            ((ICompositeCommand)_commandManager.GetCommand("Close")).RegisterAction(() => { flyout.IsOpen = false; });

#pragma warning disable AvoidAsyncVoid
            flyout.IsOpenChanged += async (sender, e) =>
#pragma warning restore AvoidAsyncVoid
            {
                if (!flyout.IsOpen)
                {
                    var vmContainer = flyout.Content as IViewModelContainer;
                    if (vmContainer != null)
                    {
                        var vm = vmContainer.ViewModel;
                        if (vm != null)
                        {
                            switch (unloadBehavior)
                            {
                                case UnloadBehavior.CloseViewModel:
                                    await vm.CloseViewModelAsync(null);
                                    break;

                                case UnloadBehavior.SaveAndCloseViewModel:
                                    await vm.SaveAndCloseViewModelAsync();
                                    break;

                                case UnloadBehavior.CancelAndCloseViewModel:
                                    await vm.CancelAndCloseViewModelAsync();
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException("unloadBehavior");
                            }
                        }
                    }

                    flyout.Content = null;
                    flyout.DataContext = null;
                }
            };

            _flyouts[name] = flyoutInfo;
        }


        public void ShowFlyout(string name, object dataContext)
        {
            Argument.IsNotNullOrWhitespace(() => name);

            if (!_flyouts.ContainsKey(name))
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Flyout '{0}' is not added yet", name);
            }

            var flyoutInfo = _flyouts[name];
            var flyout = flyoutInfo.Flyout;

            flyout.Dispatcher.BeginInvoke(() =>
            {
                var flyoutsControl = flyout.Parent as FlyoutsControl;
                if (flyoutsControl != null)
                {
                    flyoutsControl.SetCurrentValue(System.Windows.Controls.Control.BorderThicknessProperty, new Thickness(1));
                    flyoutsControl.SetCurrentValue(System.Windows.Controls.Control.BorderBrushProperty, ThemeHelper.GetAccentColorBrush());
                }

                flyout.SetCurrentValue(System.Windows.Controls.ContentControl.ContentProperty, flyoutInfo.Content);
                flyout.SetValue(FrameworkElement.DataContextProperty, dataContext);
                flyout.SetCurrentValue(Flyout.IsOpenProperty, true);
            });
        }

        public void HideFlyout(string name)
        {
            Argument.IsNotNullOrWhitespace(() => name);

            if (!_flyouts.ContainsKey(name))
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Flyout '{0}' is not added yet", name);
            }

            var flyoutInfo = _flyouts[name];
            var flyout = flyoutInfo.Flyout;

            flyout.BeginInvoke(() =>
            {
                flyout.SetCurrentValue(Flyout.IsOpenProperty, false);
            });
        }
        #endregion
    }
}