// MvxSavedStateConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Droid.Views
{
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