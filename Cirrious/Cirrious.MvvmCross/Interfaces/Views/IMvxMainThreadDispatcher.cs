﻿// IMvxMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System;

#endregion

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxMainThreadDispatcher
    {
        bool RequestMainThreadAction(Action action);
    }
}