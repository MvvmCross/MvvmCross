namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type ThreadUtilsPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.ThreadUtils.PluginLoader, MvvmCross.Plugins.ThreadUtils.iOS.Plugin>()