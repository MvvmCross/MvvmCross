using System.Linq;
using System.Windows;
using Cirrious.MvvmCross.Wpf.Views;

namespace TwitterSearch.UI.Wpf
{
    public class MultiRegionPresenter
        : MvxWpfViewPresenter
    {
        private readonly MainWindow _mainWindow;

        public MultiRegionPresenter(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Present(FrameworkElement frameworkElement)
        {
            // this is really hacky - do it using attributes isnt
            var attribute = frameworkElement
                                .GetType()
                                .GetCustomAttributes(typeof (RegionAttribute), true)
                                .FirstOrDefault() as RegionAttribute;

            var regionName = attribute == null ? null : attribute.Name;
            _mainWindow.PresentInRegion(frameworkElement, regionName);
        }  
    }
}