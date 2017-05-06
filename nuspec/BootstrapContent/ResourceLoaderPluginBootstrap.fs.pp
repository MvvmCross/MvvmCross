namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type ResourceLoaderPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.ResourceLoader.PluginLoader>()