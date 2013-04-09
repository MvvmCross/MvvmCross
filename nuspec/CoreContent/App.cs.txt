namespace YourNamespace.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<ViewModels.FirstViewModel>();
        }
    }
}