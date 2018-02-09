// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.


using MvvmCross.Converters;
using MvvmCross.Plugins;

namespace MvvmCross.Plugin.PictureChooser.Platform.iOS
{
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxPictureChooserTask, MvxImagePickerTask>();
            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(RegisterValueConverter);
        }

        private void RegisterValueConverter()
        {
            Mvx.Resolve<IMvxValueConverterRegistry>().AddOrOverwriteFrom(GetType().Assembly);
        }
    }
}
