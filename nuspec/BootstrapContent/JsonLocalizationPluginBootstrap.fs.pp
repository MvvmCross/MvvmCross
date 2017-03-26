namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type JsonLocalizationPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.JsonLocalization.PluginLoader>()