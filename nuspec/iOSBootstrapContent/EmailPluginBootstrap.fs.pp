namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type EmailPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Email.PluginLoader, MvvmCross.Plugins.Email.iOS.Plugin>()