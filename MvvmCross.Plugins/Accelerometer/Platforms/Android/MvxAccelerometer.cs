// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Hardware;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android;
using Object = Java.Lang.Object;

namespace MvvmCross.Plugin.Accelerometer.Platforms.Android
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

            var globals = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>();
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
