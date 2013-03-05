using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterSearch.UI.Wpf
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            //app.InitializeComponent();
            Uri resourceLocater = new Uri("/TwitterSearch.UI.Wpf;component/App.xaml", System.UriKind.Relative);
            System.Windows.Application.LoadComponent(app, resourceLocater);

            var ourWindow = new MainWindow();
            var presenter = new MultiRegionPresenter(ourWindow);
            var setup = new Setup(app.Dispatcher, presenter);
            setup.Initialize();
            app.MainWindow.Show();
            app.Run();
        }
    }
}
