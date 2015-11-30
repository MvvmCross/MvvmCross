// MvxUnconventionalAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Droid.Support.V7.Fragging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxFragmentAttribute : Attribute
    {
        public MvxFragmentAttribute()
        {
            
        }

        /// <summary>
        /// That shall be used only if you are using non generic fragments.
        /// </summary>
        /// <param name="viewModelType"></param>
        public MvxFragmentAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        internal Type ViewModelType { get; set; }

        public bool IsCacheableFragment => true;
    }
}