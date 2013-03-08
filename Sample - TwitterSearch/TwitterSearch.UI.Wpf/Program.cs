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
            // if Call InitializeComponent to ensure any App.Xaml resources are loaded 
            //app.InitializeComponent();
            var ourWindow = new MainWindow();
            var presenter = new MultiRegionPresenter(ourWindow);
            var setup = new Setup(app.Dispatcher, presenter);
            setup.Initialize();
            app.MainWindow.Show();
            app.Run();
        }
    }
}
