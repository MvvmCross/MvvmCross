// MvxUnconventionalAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Droid.Support.V7.Fragging.Caching;

namespace MvvmCross.Droid.Support.V7.Fragging.Attributes
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
		public Type ViewModelType { get; set; }

		/// <summary>
		/// Indicates if the fragment can be cached. True by default.
		/// </summary>
		public bool IsCacheableFragment { get; set; } = true;

		/// <summary>
		/// Use to present fragment on a parent activity of type IMvxFragmentHost
		/// TODO: Maybe support setting ViewModel which opens parent Activity as parent
		/// </summary>
		public Type ParentType { get; set; }
    }
}