// <copyright file="MvxAccelerometer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com - Hire me - I'm worth it!

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using System;

namespace MvvmCross.Plugins.Accelerometer.Wpf
{
    public class MvxAccelerometer : IMvxAccelerometer
    {
        // TODO - mahybe try WindowsAPICodePack from http://msdn.microsoft.com/en-us/windows7trainingcourse_win7sensorsmanaged_topic2.aspx
        //private Windows.Devices.Sensors.Accelerometer _accelerometer;

        public void Start()
        {
            /*
            if (_accelerometer != null)
            {
                throw new MvxException("Accelerometer already started");
            }
            _accelerometer = Windows.Devices.Sensors.Accelerometer.GetDefault();
            _accelerometer.ReadingChanged += AccelerometerOnReadingChanged;
             */
        }

        public void Stop()
        {
            /*
            if (_accelerometer == null)
            {
                throw new MvxException("Accelerometer not started");
            }
            _accelerometer.ReadingChanged -= AccelerometerOnReadingChanged;
            _accelerometer = null;
            */
        }

        /*
        private void AccelerometerOnReadingChanged(Windows.Devices.Sensors.Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            var handler = ReadingAvailable;

            if (handler == null)
                return;

            var reading = ToReading(args.Reading);

            handler(this, new MvxValueEventArgs<Reading>(reading));
        }

        private static MvxAccelerometerReading ToReading(AccelerometerReading sensorReading)
        {
            var reading = new MvxAccelerometerReading
                {
                    X = sensorReading.AccelerationX,
                    Y = sensorReading.AccelerationY,
                    Z = sensorReading.AccelerationZ,
                };
            return reading;
        }
        */

        /* _accelerometer != null; */
        public bool Started => true;

        public MvxAccelerometerReading LastReading
        {
            get
            {
                try
                {
                    return null;
                    //var reading = ToReading(_accelerometer.GetCurrentReading());
                    //return reading;
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