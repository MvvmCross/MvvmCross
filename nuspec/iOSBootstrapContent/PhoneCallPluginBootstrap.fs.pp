namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type PhoneCallPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.PhoneCall.PluginLoader, MvvmCross.Plugins.PhoneCall.iOS.Plugin>()