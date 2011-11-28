using System.ComponentModel;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows;
using Microsoft.Phone.Shell;
using Phone7.Fx.Ioc;
using Phone7.Fx.Navigation;
namespace Phone7.Fx
{
    public class PhoneBootstrapper
    {
        public Application Application { get; protected set; }

        private PhoneApplicationService _phoneService;
        private bool _phoneApplicationInitialized = false;
        private PhoneApplicationFrame _rootFrame;

        public PhoneBootstrapper()
        {
            if (DesignerProperties.IsInDesignTool)
                StartDesignTime();
            else StartRuntime();
        }

        protected void PrepareApplication()
        {
            _phoneService = new PhoneApplicationService();
            _phoneService.Activated += OnActivated;
            _phoneService.Deactivated += OnDeactivated;
            _phoneService.Launching += OnLaunching;
            _phoneService.Closing += OnClosing;

            Application.ApplicationLifetimeObjects.Add(_phoneService);
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        public virtual void OnLaunching(object sender, LaunchingEventArgs e)
        {

        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        public virtual void OnActivated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        public virtual void OnDeactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        public virtual void OnClosing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        protected virtual void NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    // A navigation has failed; break into the debugger
            //    System.Diagnostics.Debugger.Break();
            //}
        }

        // Code to execute on Unhandled Exceptions
        protected virtual void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    // An unhandled exception has occurred; break into the debugger
            //    System.Diagnostics.Debugger.Break();
            //}
        }

        #region Phone application initialization

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (_phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            _rootFrame = this.PhoneApplicationFrame;
            _rootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            _rootFrame.NavigationFailed += NavigationFailed;

            // Ensure we don't initialize again
            _phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (Application.RootVisual != _rootFrame)
                Application.RootVisual = _rootFrame;

            // Remove this handler since it is no longer needed
            _rootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion


        protected virtual void StartDesignTime() { }

        /// <summary>
        /// Starts the runtime.
        /// </summary>
        protected virtual void StartRuntime()
        {
            Application = Application.Current;
            PrepareApplication();
            Application.UnhandledException += OnUnhandledException;

            InitializePhoneApplication();
            AfterInitializePhoneApplication();

            RegisterPhoneServices();
            Configure();
        }

        protected virtual void AfterInitializePhoneApplication()
        {
            
        }

        /// <summary>
        /// Registers the phone services.
        /// </summary>
        private void RegisterPhoneServices()
        {
            Container.Current.RegisterInstance(typeof(INavigationService), new Phone7.Fx.Navigation.NavigationService(this._rootFrame));
        }

        /// <summary>
        /// Gets the phone application frame.
        /// </summary>
        public virtual PhoneApplicationFrame PhoneApplicationFrame
        {
            get
            {
                return new PhoneApplicationFrame();
            }
        }


        /// <summary>
        /// Configures this instance.
        /// </summary>
        protected virtual void Configure() { }
    }
}