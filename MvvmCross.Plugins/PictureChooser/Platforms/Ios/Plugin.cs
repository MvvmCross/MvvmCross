// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Converters;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Ios
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.RegisterType<IMvxPictureChooserTask, MvxImagePickerTask>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(RegisterValueConverter);
        }

        private void RegisterValueConverter()
        {
            Mvx.IoCProvider.Resolve<IMvxValueConverterRegistry>().AddOrOverwriteFrom(GetType().Assembly);
        }
    }
}
