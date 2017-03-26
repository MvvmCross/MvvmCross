namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type PhoneCallPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.PhoneCall.PluginLoader>()