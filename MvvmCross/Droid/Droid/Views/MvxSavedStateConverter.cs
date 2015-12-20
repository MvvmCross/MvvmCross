// MvxSavedStateConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using System;
    using System.Collections.Generic;

    using Android.OS;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Droid.Platform;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;

    public class MvxSavedStateConverter : IMvxSavedStateConverter
    {
        private const string ExtrasKey = "MvxSaved";

        public IMvxBundle Read(Bundle bundle)
        {
            var extras = bundle?.GetString(ExtrasKey);
            if (string.IsNullOrEmpty(extras))
                return null;

            try
            {
                var converter = Mvx.Resolve<IMvxNavigationSerializer>();
                var data = converter.Serializer.DeserializeObject<Dictionary<string, string>>(extras);
                return new MvxBundle(data);
            }
            catch (Exception)
            {
                MvxTrace.Error("Problem getting the saved state - will return null - from {0}",
                               extras);
                return null;
            }
        }

        public void Write(Bundle bundle, IMvxBundle savedState)
        {
            if (savedState == null)
                return;

            if (savedState.Data.Count == 0)
                return;

            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var data = converter.Serializer.SerializeObject(savedState.Data);
            bundle.PutString(ExtrasKey, data);
        }
    }
}