using MvvmCross.ViewModels;

namespace Playground.Droid
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            // register the appstart object
            RegisterCustomAppStart<AppStart>();
        }
    }
}
