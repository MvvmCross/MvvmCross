using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace BestSellers.Droid
{
    public class ErrorDisplayer
        : IMvxServiceConsumer
    {
        private readonly Context _applicationContext;

        public ErrorDisplayer(Context applicationContext)
        {
            _applicationContext = applicationContext;

            var source = this.GetService<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            var inflater = LayoutInflater.FromContext(_applicationContext);
            View layoutView = inflater.Inflate(Resource.Layout.ToastLayout_Error, null);
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