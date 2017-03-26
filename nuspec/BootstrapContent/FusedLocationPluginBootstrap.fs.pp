namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type LocationPluginBootstrap
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Location.PluginLoader, MvvmCross.Plugins.Location.Fused.Droid.Plugin>()