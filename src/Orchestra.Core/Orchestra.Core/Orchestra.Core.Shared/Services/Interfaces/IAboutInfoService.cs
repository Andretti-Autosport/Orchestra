﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAboutInfoService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Models;

    public interface IAboutInfoService
    {
        /// <summary>
        /// Returns the about info. If <c>null</c>, the shell will not show the about window.
        /// </summary>
        /// <returns></returns>
        AboutInfo GetAboutInfo();
    }
}