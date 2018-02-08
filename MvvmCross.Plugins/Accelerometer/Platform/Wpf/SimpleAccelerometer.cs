// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base.Core;
using MvvmCross.Base.Exceptions;

namespace MvvmCross.Plugin.Accelerometer.Platform.Wpf
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
