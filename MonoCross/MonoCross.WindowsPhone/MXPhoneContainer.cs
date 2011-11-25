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
        private readonly Dictionary<Type, object> _viewModels = new Dictionary<Type, object>();
        private readonly PhoneApplicationFrame _rootFrame;

        public MXPhoneContainer(MXApplication theApp, PhoneApplicationFrame frame)
            : base(theApp)
        {
            _rootFrame = frame;
        }

        public override void ShowError(IMXView fromView, IMXController controller, Exception exception)
        {
            _rootFrame.Dispatcher.BeginInvoke(() => MessageBox.Show("Soz - I haz a prblm - " + exception.Message));
        }

        protected void StartViewForController(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            Type viewType = PhoneContainerInstance.Views.GetViewType(viewPerspective);
            if (viewType == null)
            {
                Console.WriteLine("View not found for " + viewPerspective.ToString());
                throw new TypeLoadException("View not found for " + viewPerspective.ToString());
            }

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
            // TODO - Stuart changed this - what's going on ?!
            _viewModels.Clear();
            _viewModels[controller.ModelType] = controller.GetModel();

            var page = fromView as PhoneApplicationPage;
            if (page != null)
            {
                // NOTE: assumes XAML file matches type name and no sub directories
                page.NavigationService.Navigate(viewUri);
            }
            else
            {
                if (_rootFrame != null)
                {
                    _rootFrame.Navigate(viewUri);
                }

                // failure, called too early or Something Very Bad Happened(tm)...
            }
        }

        public bool TryGetViewModel(Type modelType, out object viewModel)
        {
            return _viewModels.TryGetValue(modelType, out viewModel);
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => { StartViewForController(fromView, controller, viewPerspective); });
        }

        public override void Redirect(string url)
        {
            Navigate(null, url);
            CancelLoad = true;
        }
    }
}