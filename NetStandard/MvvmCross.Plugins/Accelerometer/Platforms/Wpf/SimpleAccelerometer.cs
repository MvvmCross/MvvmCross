// <copyright file="MvxAccelerometer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com - Hire me - I'm worth it!

using System;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Plugins.Accelerometer.Wpf
{
    public class MvxAccelerometer : IMvxAccelerometer
    {
        // TODO - mahybe try WindowsAPICodePack from http://msdn.microsoft.com/en-us/windows7trainingcourse_win7sensorsmanaged_topic2.aspx
        public void Start()
        {
        }

        public void Stop()
        {
        }

        public bool Started => true;

        public MvxAccelerometerReading LastReading
        {
            get
            {
                try
                {
                    return null;
                }
                catch (Exception exception)
                {
                    throw exception.MvxWrap("Problem getting current Accelerometer reading");
                }
            }
        }

        public event EventHandler<MvxValueEventArgs<MvxAccelerometerReading>> ReadingAvailable;
    }
}