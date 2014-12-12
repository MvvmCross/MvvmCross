using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Animation
{
    public interface IPageAnimation
    {
        Task NavigatedToForward(FrameworkElement source);
        Task NavigatedToBackward(FrameworkElement source);
        Task NavigatingFromForward(FrameworkElement source);
        Task NavigatingFromBackward(FrameworkElement source);
    }
}
