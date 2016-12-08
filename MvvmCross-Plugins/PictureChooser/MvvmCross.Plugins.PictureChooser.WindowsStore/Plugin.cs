// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

#if WINDOWS_STORE
namespace MvvmCross.Plugins.PictureChooser.WindowsStore
#endif
#if WINDOWS_UWP
namespace MvvmCross.Plugins.PictureChooser.WindowsUWP
#endif
{
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxPictureChooserTask, MvxPictureChooserTask>();
        }
    }
}