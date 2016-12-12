namespace MvvmCross.iOS.Support.JASidePanelsSample.Core
{
    using MvvmCross.Core.ViewModels;
    using Platform.IoC;

    public class App : MvxApplication
    {
        /// <summary>The initialize.</summary>
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart(new AppStart());
        }
    }
}