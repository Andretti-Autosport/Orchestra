﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.ViewModels
{
    using Catel.MVVM;
    using Catel.Reflection;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            var assembly = GetType().Assembly;

            Title = string.Format("{0} v{1}", assembly.Title(), assembly.Version());
        }
    }
}