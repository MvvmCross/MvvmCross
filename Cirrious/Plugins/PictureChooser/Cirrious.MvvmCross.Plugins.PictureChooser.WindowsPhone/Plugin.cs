using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.WindowsPhone.Platform.Tasks;

namespace Cirrious.MvvmCross.Plugins.Location.WindowsPhone
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