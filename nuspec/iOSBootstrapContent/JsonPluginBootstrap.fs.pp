namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type JsonPluginBootstrap() = 
	inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Json.PluginLoader>()