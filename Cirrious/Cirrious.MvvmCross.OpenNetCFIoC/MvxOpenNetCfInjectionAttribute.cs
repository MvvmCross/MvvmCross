// MvxOpenNetCfInjectionAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region Credit - OpenNetCf

// This file is based on the OpenNetCf IoC container - used under free license -see http://ioc.codeplex.com

#endregion

using System;

namespace Cirrious.MvvmCross.OpenNetCfIoC
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class MvxOpenNetCfInjectionAttribute : Attribute
    {
    }
}