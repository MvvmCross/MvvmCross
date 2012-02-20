using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Android.Platform;
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