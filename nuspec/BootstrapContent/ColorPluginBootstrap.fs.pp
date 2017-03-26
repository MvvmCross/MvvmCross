namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type ColorPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Color.PluginLoader>()