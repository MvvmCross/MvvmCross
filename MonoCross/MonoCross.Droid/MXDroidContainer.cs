using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;

using MonoCross.Navigation;

namespace MonoCross.Droid
{
    public class MXDroidContainer : MXContainer
    {
        public static MXDroidContainer DroidInstance { get { return MXContainer.Instance as MXDroidContainer; } }

        public Dictionary<Type, object> ViewModels = new Dictionary<Type, object>();
        public Action<Type> NavigationHandler { get; set; }
        public Context ApplicationContext { get; private set; }

        public MXDroidContainer(MXApplication theApp, Context applicationContext)
            : base(theApp)
        {
            ApplicationContext = applicationContext;
        }

        public static void Initialize(MXApplication theApp, Context applicationContext)
        {
            MXContainer.InitializeContainer(new MXDroidContainer(theApp, applicationContext));
            MXContainer.Instance.ThreadedLoad = true;
        }

        protected override void OnControllerLoadBegin(IMXController controller)
        {
            Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadBegin");
        }

        protected override void OnControllerLoadFailed(IMXController controller, Exception ex)
        {
            Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadFailed: " + ex.Message);
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete");

            Type viewType = Views.GetViewType(viewPerspective);
            if (viewType != null)
            {
                // stash the model away so we can get it back when the view shows up!
                ViewModels[controller.ModelType] = controller.GetModel();

                Activity activity = fromView as Activity;
                if (NavigationHandler != null)
                {
                    // allow first crack at the view creation to the person over-riding
                    NavigationHandler(viewType);
                }
                else if (activity != null)
                {
                    // use the context we have to start the next view
                    Intent intent = new Intent(activity, viewType);
                    intent.AddFlags(ActivityFlags.NewTask);
                    activity.StartActivity(intent);
                }
                else if (ApplicationContext != null)
                {
                    // use the application context to instantiate the new new
                    Intent intent = new Intent(ApplicationContext, viewType);
                    intent.AddFlags(ActivityFlags.NewTask);
                    ApplicationContext.StartActivity(intent);
                }
                else
                {
                    Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete: View not found for " + viewPerspective.ToString());
                    throw new TypeLoadException("View not found for " + viewPerspective.ToString());
                }
            }
            else
            {
                Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete: View not found for " + viewPerspective.ToString());
                throw new TypeLoadException("View not found for " + viewPerspective.ToString());
            }
        }

        public override void Redirect(string url)
        {
            Navigate(null, url);
            CancelLoad = true;
        }
    }
}