namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type MessengerPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Messenger.PluginLoader>()