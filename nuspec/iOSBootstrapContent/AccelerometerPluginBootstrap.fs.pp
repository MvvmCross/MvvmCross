namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type AccelerometerPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Accelerometer.PluginLoader, MvvmCross.Plugins.Accelerometer.iOS.Plugin>()