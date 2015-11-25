// <copyright file="Plugin.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com - Hire me - I'm worth it!

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace MvvmCross.Plugins.Accelerometer.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMvxAccelerometer>(new MvxAccelerometer());
        }
    }
}