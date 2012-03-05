using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.WindowsPhone.Views.Lessons
{
    public partial class TipView : BaseTipView
    {
        public TipView()
        {
            InitializeComponent();
        }
    }

    public class BaseTipView : MvxPhonePage<TipViewModel> { }
}