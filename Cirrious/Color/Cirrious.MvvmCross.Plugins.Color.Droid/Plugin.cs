// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.UI;
using Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets;

namespace Cirrious.MvvmCross.Plugins.Color.Droid
{
    public class Plugin
        : IMvxPlugin
          , IMvxConsumer
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterSingleton<IMvxNativeColor>(new MvxAndroidColor());
            RegisterDefaultBindings();
        }

        private void RegisterDefaultBindings()
        {
            var helper = new MvxDefaultColorBindingSet();
            helper.RegisterBindings();
        }

        #endregion
    }
}