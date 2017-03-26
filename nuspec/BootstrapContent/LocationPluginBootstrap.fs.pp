namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type LocationPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Location.PluginLoader>()