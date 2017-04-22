using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;

namespace Eventhooks.Core
{
	public class App : MvxApplication
	{
		public override void Initialize()
		{
			CreatableTypes()
				.EndingWith("Service")
				.AsInterfaces()
				.RegisterAsLazySingleton();

			RegisterAppStart<ViewModels.FirstViewModel>();
		}
	}
}
