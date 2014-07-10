using Cirrious.CrossCore.IoC;
using FirstDemo.Core.ViewModels;

namespace FirstDemo.Core
{
	public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
	{
		public override void Initialize ()
		{
			CreatableTypes ()
				.EndingWith ("Service")
				.AsInterfaces ()
				.RegisterAsLazySingleton ();
			RegisterAppStart<FirstViewModel> ();
		}
	}
}