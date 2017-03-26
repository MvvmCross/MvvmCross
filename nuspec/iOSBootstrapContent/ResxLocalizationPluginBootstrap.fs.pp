namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type ResxLocalizationPluginBootstrap() =
     inherit MvxPluginBootstrapAction<MvvmCross.Plugins.ResxLocalization.PluginLoader>()