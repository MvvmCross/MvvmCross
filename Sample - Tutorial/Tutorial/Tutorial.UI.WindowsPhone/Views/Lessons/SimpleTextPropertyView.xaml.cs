using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class SimpleTextPropertyView : BaseSimpleTextPropertyView
    {
        public SimpleTextPropertyView()
        {
            InitializeComponent();
        }
    }

    public class BaseSimpleTextPropertyView : MvxPhonePage<SimpleTextPropertyViewModel> { }
}