namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type AccelerometerPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Accelerometer.PluginLoader>()