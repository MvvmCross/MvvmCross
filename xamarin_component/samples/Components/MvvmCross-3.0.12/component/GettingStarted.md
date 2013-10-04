TODO Core
The steps to get this Core project working are:

1. Create a ViewModels folder and add a ViewModel:

    public class FirstViewModel : Cirrious.MvvmCross.ViewModels.MvxViewModel
    {
         public string Hello { get { return "Hello MvvmCross"; } }
    }

2. Add an App.cs class - in it place the code to start the app with the FirstViewModel:

    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
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

TODO Droid
The steps to get this Android UI working are:

1. Add a reference to your Core PCL project
2. Create a Setup.cs file
3. Add the MvxBindingAttributes file - make sure it's type is set to 'AndroidResource'
4. Add a splashscreen - both cs file and the axml to the Resources folder 
  - also remove any other `MainLauncher` activities - eg Activity1
5. Add a views folder and a FirstView view - also add FirstView.axml to the resources folder


TODO Touch UI
NOTE 
- Touch won't currently build from the PC as Xamarin.iOS is currently in the lab under development.
- To make it build on the Mac you will need to substitute the 'real PCLs' with versions built using Xamarin.iOS on the Mac

The steps to get this Touch UI working are:

1. Add a reference to your Core PCL project
2. Create a Setup.cs file
3. Modify AppDelegate.cs to create the new Setup and to call the IMvxAppStart
4. Add a views folder and a view - cs - change the UIViewController inheritance to MvxViewController
5. Edit the ViewDidLoad in the cs to add a bound control - e.g:

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();
            var uiLabel = new UILabel(new RectangleF(0, 0, 320, 100));
            View.AddSubview(uiLabel);

            this.CreateBinding(uiLabel).To<FirstViewModel>(vm => vm.Hello).Apply();

            // Perform any additional setup after loading the view
        }

Where this requires using's of:

    using YourNameSpace.Core.ViewModels;
    using Cirrious.MvvmCross.Binding.BindingContext;
    using Cirrious.MvvmCross.Touch.Views;
