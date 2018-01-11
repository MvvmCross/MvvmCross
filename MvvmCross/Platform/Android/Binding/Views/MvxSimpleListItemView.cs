using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MvvmCross.Binding.Droid.Views
{
    [Register("mvvmcross.binding.droid.views.MvxSimpleListItemView")]
    public class MvxSimpleListItemView : MvxListItemView
    {
        public MvxSimpleListItemView(Context context, IMvxLayoutInflaterHolder layoutInflaterHolder, 
            object dataContext, ViewGroup parent, int templateId) 
            : base(context, layoutInflaterHolder, dataContext, parent, templateId)
        {
        }

        public override object DataContext
        {
            get => base.DataContext;
            set
            {
                var context = base.DataContext = value;
                var textView = Content as TextView;
                if (textView != null)
                    textView.Text = context?.ToString();
            }
        }
    }
}