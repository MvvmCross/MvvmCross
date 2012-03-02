#region Copyright
// <copyright file="MvxBaseAndroidTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Android.Target
{
    public abstract class MvxBaseAndroidTargetBinding 
        : MvxBaseTargetBinding
          , IMvxServiceConsumer<IMvxAndroidGlobals>
    {
        private IMvxAndroidGlobals _androidGlobals;
        protected IMvxAndroidGlobals AndroidGlobals
        {
            get
            {
                if (_androidGlobals == null)
                    _androidGlobals = this.GetService<IMvxAndroidGlobals>();
                return _androidGlobals;
            }
        }
    }
}