// MvxRegionAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.WindowsUWP.Views
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class MvxRegionAttribute
        : Attribute
    {
        public MvxRegionAttribute(string regionName)
        {
            Name = regionName;
        }

        public string Name { get; private set; }
    }
}