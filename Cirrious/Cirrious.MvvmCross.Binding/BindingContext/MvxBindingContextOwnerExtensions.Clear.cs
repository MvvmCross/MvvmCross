// MvxBindingContextOwnerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
	public static partial class MvxBindingContextOwnerExtensions
    {
		public static void ClearBindings(this IMvxBindingContextOwner owner, object target)
        {
            owner.BindingContext.ClearBindings(target);
        }

        public static void ClearAllBindings(this IMvxBindingContextOwner owner)
        {
            owner.BindingContext.ClearAllBindings();
        }
    }
}