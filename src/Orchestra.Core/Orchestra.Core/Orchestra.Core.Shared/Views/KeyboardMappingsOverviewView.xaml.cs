﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMappingsOverviewView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Windows;
    using Catel.Windows;
    using Catel.Windows.Controls;

    /// <summary>
    /// Interaction logic for KeyboardMappingsOverviewView.xaml.
    /// </summary>
    public partial class KeyboardMappingsOverviewView : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsOverviewView"/> class.
        /// </summary>
        public KeyboardMappingsOverviewView()
        {
            InitializeComponent();

            //StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);
        }
        #endregion
    }
}