using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Interfaces;

namespace BestSellers.Droid
{
    public class ErrorDisplayer
        : IMvxConsumer
    {
        private readonly Context _applicationContext;

        public ErrorDisplayer(Context applicationContext)
        {
            _applicationContext = applicationContext;

            var source = this.Resolve<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            var activity = this.Resolve<IMvxAndroidCurrentTopActivity>().Activity as IMvxBindingContextOwner;
            // note that we're not using Binding in this Inflation - but the overhead is minimal - so use it anyway!
            View layoutView = activity.BindingInflate(Resource.Layout.ToastLayout_Error, null);
            var text1 = layoutView.FindViewById<TextView>(Resource.Id.ErrorText1);
            text1.Text = "Sorry!";
            var text2 = layoutView.FindViewById<TextView>(Resource.Id.ErrorText2);
            text2.Text = message;

            var toast = new Toast(_applicationContext);
            toast.SetGravity(GravityFlags.CenterVertical, 0, 0);
            toast.Duration = ToastLength.Long;
            toast.View = layoutView;
            toast.Show();
        }
    }
}