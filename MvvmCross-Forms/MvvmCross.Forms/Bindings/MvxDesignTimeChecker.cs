using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.IoC;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings
{
    public static class MvxDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            if (_checked)
                return;

            _checked = true;

            // if Application.Current == null Forms is in design mode
            if (Application.Current != null)
                return;

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxSimpleIoCContainer.Initialize();
                Mvx.RegisterSingleton(iocProvider);
            }

            if (!Mvx.CanResolve<IMvxBindingParser>())
            {
                //We might want to look into returning the platform specific Forms binder
                var builder = new MvxBindingBuilder();
                builder.DoRegistration();
            }
        }
    }
}
