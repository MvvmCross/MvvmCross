using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Phone.Controls;

using MonoCross.Navigation;
using System.Windows.Threading;
using System.Windows.Navigation;

namespace MonoCross.WindowsPhone
{
    public class MXPhoneContainer: MXContainer
    {
        public static void Initialize(MXApplication theApp, PhoneApplicationFrame rootFrame)
        {
            MXContainer.InitializeContainer(new MXPhoneContainer(theApp, rootFrame));
        }

        public static MXPhoneContainer PhoneContainerInstance { get { return Instance as MXPhoneContainer; } }
        private readonly PhoneApplicationFrame _rootFrame;
        private object _cachedViewModel = null;

        public MXPhoneContainer(MXApplication theApp, PhoneApplicationFrame frame)
            : base(theApp)
        {
            _rootFrame = frame;
            _rootFrame.Loaded += new RoutedEventHandler(_rootFrame_Loaded);
        }

        public override void ShowError(IMXView fromView, IMXController controller, Exception exception)
        {
            _rootFrame.Dispatcher.BeginInvoke(() => MessageBox.Show("Soz - I haz a prblm - " + exception.Message));
        }

        protected void StartViewForController(IMXView fromView, IMXController controller, MXShowViewRequest showViewRequest)
        {
            var viewPerspective = showViewRequest.ViewPerspective;
            Type viewType = PhoneContainerInstance.Views.GetViewType(viewPerspective);
            if (viewType == null)
            {
                throw new TypeLoadException("View not found for " + viewPerspective.ToString());
            }

#warning TODO - make this Uri look up better!
            Uri viewUri = new Uri("/" + viewType.Name + ".xaml", UriKind.Relative);
            
            // get the uri from the MXPhoneView attribute, if present
            object[] attributes = viewType.GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] is MXPhoneViewAttribute)
                {
                    viewUri = new Uri(((MXPhoneViewAttribute)attributes[i]).Uri, UriKind.Relative);
                    break;
                }
            }           
            
            // stash the model away so we can get it back when the view shows up!
            _cachedViewModel = showViewRequest.ViewModel;

#warning TODO - make this navigation better - pwn the history stack
            //var page = fromView as PhoneApplicationPage;
            //((MXPhonePage)fromView).NavigationService.n

            _rootFrame.Source = viewUri;

            //_rootFrame.Navigate(viewUri);
        }

        void _rootFrame_Loaded(object sender, RoutedEventArgs e)
        {
            _rootFrame.RemoveBackEntry();
        }


        public bool TryGetViewModel(Type modelType, out object viewModel)
        {
            viewModel = null;

            if (_cachedViewModel == null)
                return false;

            if (_cachedViewModel.GetType() != modelType)
                return false;

            viewModel = _cachedViewModel;
            return true;
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXShowViewRequest showViewRequest)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => { StartViewForController(fromView, controller, showViewRequest); });
        }

        public override void Redirect(string url)
        {
            Navigate(null, url);
            CancelLoad = true;
        }
    }
}