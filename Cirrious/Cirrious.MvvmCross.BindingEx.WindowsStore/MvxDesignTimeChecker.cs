// MvxDesignTimeChecker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.BindingEx.WindowsStore
{
    public static class MvxDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            if (_checked)
                return;

            _checked = true;
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;

            var iocProvider = MvxSimpleIoCContainer.Initialise();
            Mvx.RegisterSingleton<IMvxIoCProvider>(iocProvider);

            var builder = new MvxStoreBindingBuilder();
            builder.DoRegistration();
        }
    }
}