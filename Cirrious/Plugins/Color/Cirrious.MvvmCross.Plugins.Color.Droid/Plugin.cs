// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets;

namespace Cirrious.MvvmCross.Plugins.Color.Droid
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceConsumer
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceInstance<IMvxNativeColor>(new MvxAndroidColor());
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