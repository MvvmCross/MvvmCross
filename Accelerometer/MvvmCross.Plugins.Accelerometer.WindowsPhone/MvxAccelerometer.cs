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
using Microsoft.Devices.Sensors;
using System;

namespace MvvmCross.Plugins.Accelerometer.WindowsPhone
{
    public class MvxAccelerometer : IMvxAccelerometer
    {
        private Microsoft.Devices.Sensors.Accelerometer _accelerometer;

        public void Start()
        {
            if (_accelerometer != null)
            {
                throw new MvxException("Accelerometer already started");
            }
            _accelerometer = new Microsoft.Devices.Sensors.Accelerometer();
            _accelerometer.CurrentValueChanged += AccelerometerOnCurrentValueChanged;
            _accelerometer.Start();
        }

        public void Stop()
        {
            if (_accelerometer == null)
            {
                throw new MvxException("Accelerometer not started");
            }
            _accelerometer.Stop();
            _accelerometer = null;
        }

        private void AccelerometerOnCurrentValueChanged(object sender,
                                                        SensorReadingEventArgs<AccelerometerReading>
                                                            sensorReadingEventArgs)
        {
            var handler = ReadingAvailable;

            if (handler == null)
                return;

            var reading = ToReading(sensorReadingEventArgs.SensorReading);

            handler(this, new MvxValueEventArgs<MvxAccelerometerReading>(reading));
        }

        private static MvxAccelerometerReading ToReading(AccelerometerReading sensorReading)
        {
            var reading = new MvxAccelerometerReading
            {
                X = sensorReading.Acceleration.X,
                Y = sensorReading.Acceleration.Y,
                Z = sensorReading.Acceleration.Z,
            };
            return reading;
        }

        public bool Started => _accelerometer != null;

        public MvxAccelerometerReading LastReading
        {
            get
            {
                try
                {
                    var reading = ToReading(_accelerometer.CurrentValue);
                    return reading;
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