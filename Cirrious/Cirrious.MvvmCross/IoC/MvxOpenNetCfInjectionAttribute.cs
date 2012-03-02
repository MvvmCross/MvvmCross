#region Copyright
// <copyright file="MvxOpenNetCfInjectionAttribute.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region Credit - OpenNetCf

// This file is based on the OpenNetCf IoC container - used under free license -see http://ioc.codeplex.com

#endregion

using System;

namespace Cirrious.MvvmCross.IoC
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class MvxOpenNetCfInjectionAttribute : Attribute
    {
        public MvxOpenNetCfInjectionAttribute()
        {
        }
    }
}