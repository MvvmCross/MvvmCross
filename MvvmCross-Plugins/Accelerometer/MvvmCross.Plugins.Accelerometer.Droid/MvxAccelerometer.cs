﻿// <copyright file="MvxAccelerometer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com - Hire me - I'm worth it!

using System;
using Android.Content;
using Android.Hardware;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform.Exceptions;
using Object = Java.Lang.Object;

namespace MvvmCross.Plugins.Accelerometer.Droid
{
    [Preserve(AllMembers = true)]
    public class MvxAccelerometer
        : Object, ISensorEventListener, IMvxAccelerometer
    {
        private Sensor _accelerometer;
        private SensorManager _sensorManager;

        public void Start()
        {
            if (_accelerometer != null)
            {
                throw new MvxException("Accelerometer already started");
            }

            var globals = Mvx.Resolve<IMvxAndroidGlobals>();
            _sensorManager = (SensorManager) globals.ApplicationContext.GetSystemService(Context.SensorService);
            if (_sensorManager == null)
                throw new MvxException("Failed to find SensorManager");

            _accelerometer = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            if (_accelerometer == null)
                throw new MvxException("Failed to find Accelerometer");

            // It is not necessary to get accelerometer events at a very high
            // rate, by using a slower rate (SENSOR_DELAY_UI), we get an
            // automatic low-pass filter, which "extracts" the gravity component
            // of the acceleration. As an added benefit, we use less power and
            // CPU resources.
            _sensorManager.RegisterListener(this, _accelerometer, SensorDelay.Ui);
        }

        public void Stop()
        {
            if (_accelerometer == null)
            {
                throw new MvxException("Accelerometer not started");
            }
            _sensorManager.UnregisterListener(this);
            _sensorManager = null;
            _accelerometer = null;
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // ignored
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type != SensorType.Accelerometer)
                return;

            var reading = ToReading(e.Values[0], e.Values[1], e.Values[2]);

            LastReading = reading.Clone();

            var handler = ReadingAvailable;

            handler?.Invoke(this, new MvxValueEventArgs<MvxAccelerometerReading>(reading));
        }

        private static MvxAccelerometerReading ToReading(double x, double y, double z)
        {
            var reading = new MvxAccelerometerReading
                {
                    X = x,
                    Y = y,
                    Z = z
                };
            return reading;
        }

        public bool Started => _accelerometer != null;

        public MvxAccelerometerReading LastReading { get; private set; }

        public event EventHandler<MvxValueEventArgs<MvxAccelerometerReading>> ReadingAvailable;
    }
}