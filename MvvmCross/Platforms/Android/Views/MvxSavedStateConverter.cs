// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Android.OS;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views
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
                var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
                var data = converter.Serializer.DeserializeObject<Dictionary<string, string>>(extras);
                return new MvxBundle(data);
            }
            catch (Exception)
            {
                MvxLog.Instance.Error("Problem getting the saved state - will return null - from {0}",
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

            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            var data = converter.Serializer.SerializeObject(savedState.Data);
            bundle.PutString(ExtrasKey, data);
        }
    }
}
