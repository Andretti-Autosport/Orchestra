﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hint.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    public class Hint : IHint
    {
        #region Constructors
        public Hint(string text, string controlName)
        {
            Text = text;
            ControlName = controlName;
        }
        #endregion

        #region IHint Members
        public string Text { get; private set; }
        public string ControlName { get; private set; }
        #endregion
    }
}