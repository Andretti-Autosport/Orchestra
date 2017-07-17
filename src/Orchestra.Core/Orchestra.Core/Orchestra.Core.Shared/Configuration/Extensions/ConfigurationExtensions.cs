﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Configuration
{
    using Catel;
    using Catel.Configuration;

    public static class ConfigurationExtensions
    {
        public static bool IsConfigurationKey(this ConfigurationChangedEventArgs e, string expectedKey)
        {
            return IsConfigurationKey(e.Key, expectedKey);
        }

        public static bool IsConfigurationKey(this string key, string expectedKey)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return true;
            }

            return key.EqualsIgnoreCase(expectedKey);
        }
    }
}