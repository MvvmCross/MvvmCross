﻿#region Copyright
// <copyright file="MvxAndroidResourceLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.IO;
using Android.Content.Res;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.Droid
{
    public class MvxAndroidResourceLoader 
        : MvxBaseResourceLoader
        , IMvxServiceConsumer<IMvxAndroidGlobals>
    {
        private AssetManager _assets;

        public MvxAndroidResourceLoader()
        {
        }

        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            using (var input = Assets.Open(resourcePath))
            {
                streamAction(input);
            }
        }

        #endregion

        private AssetManager Assets
        {
            get
            {
                if (_assets == null)
                {
                    _assets = this.GetService<IMvxAndroidGlobals>().ApplicationContext.Assets;
                }
                return _assets;
            }
        }
    }
}
