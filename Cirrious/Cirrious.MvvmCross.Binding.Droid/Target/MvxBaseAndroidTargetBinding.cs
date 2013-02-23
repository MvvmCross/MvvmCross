// MvxBaseAndroidTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Bindings.Target;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public abstract class MvxBaseAndroidTargetBinding
        : MvxBaseTargetBinding
          , IMvxServiceConsumer
    {
        private IMvxAndroidGlobals _androidGlobals;

        protected MvxBaseAndroidTargetBinding(object target)
            : base(target)
        {
        }

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