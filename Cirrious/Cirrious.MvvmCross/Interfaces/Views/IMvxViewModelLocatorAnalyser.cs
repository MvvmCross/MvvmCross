// IMvxViewModelLocatorAnalyser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewModelLocatorAnalyser
    {
        IEnumerable<MethodInfo> GenerateLocatorMethods(Type locatorType);
    }
}