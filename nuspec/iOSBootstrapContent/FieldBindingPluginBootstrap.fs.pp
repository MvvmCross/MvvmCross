namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type FieldBindingPluginBootstrap() = 
	inherit MvxPluginBootstrapAction<MvvmCross.Plugins.FieldBinding.PluginLoader>()