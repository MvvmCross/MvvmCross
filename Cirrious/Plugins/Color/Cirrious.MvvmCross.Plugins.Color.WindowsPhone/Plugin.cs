using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Converters;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Color.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxNativeColor>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceInstance<IMvxNativeColor>(new MvxWindowsPhoneColor());
        }

        #endregion
    }
}