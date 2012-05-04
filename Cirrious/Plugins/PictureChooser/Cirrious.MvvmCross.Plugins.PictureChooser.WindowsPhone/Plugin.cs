using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxPictureChooserTask>
        , IMvxServiceProducer<IMvxCombinedPictureChooserTask>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceType<IMvxPictureChooserTask, MvxPictureChooserTask>();
            this.RegisterServiceType<IMvxCombinedPictureChooserTask, MvxPictureChooserTask>();
        }

        #endregion
    }
}