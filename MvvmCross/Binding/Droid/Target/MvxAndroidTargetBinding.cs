// MvxAndroidTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using MvvmCross.Platform;

    public abstract class MvxAndroidTargetBinding
        : MvxConvertingTargetBinding
    {
        private IMvxAndroidGlobals _androidGlobals;

        protected MvxAndroidTargetBinding(object target)
            : base(target)
        {
        }

        protected IMvxAndroidGlobals AndroidGlobals => this._androidGlobals ?? (this._androidGlobals = Mvx.Resolve<IMvxAndroidGlobals>());
    }
}