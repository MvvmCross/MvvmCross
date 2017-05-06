namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type EmailPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Email.PluginLoader>()