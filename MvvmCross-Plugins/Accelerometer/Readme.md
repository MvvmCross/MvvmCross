### Accelerometer

The `Accelerometer` plugin provides access to a platforms accelerometer using a singleton implementing the API:

    public interface IMvxAccelerometer
    {
        void Start();
        void Stop();
        bool Started { get; }
        MvxAccelerometerReading LastReading { get; }
        event EventHandler<MvxValueEventArgs<MvxAccelerometerReading>> ReadingAvailable;
    }
    
This plugin is available on all of Android, iOS, WindowsPhone, WindowsStore and Wpf.

The Wpf implementation is an empty implementation - so it's not really supported there.

All implementations are 'intro level' only implementations. You may find that you need to add additional code to, for example, get consistent x,y,z readings across all platforms or to handle errors on devices where the accelerometer is not currently available.

Current advice (August 2013): if your app requires accelerometer support, consider this plugin as a good open source starting point - but do test the implementation on several devices to ensure it suits your app - consider developing the code further (and consider contributing your changes back to improving this plugin too!).