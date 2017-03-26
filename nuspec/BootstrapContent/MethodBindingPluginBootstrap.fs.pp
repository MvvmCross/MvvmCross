namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type MethodBindingPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.MethodBinding.PluginLoader>()