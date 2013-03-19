using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cirrious.MvvmCross.Views;

namespace TwitterSearch.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void PresentInRegion(FrameworkElement frameworkElement, string regionName)
        {
            switch (regionName)
            {
                case "Detail":
                    RightHandColumn.Children.Clear();
                    RightHandColumn.Children.Add(frameworkElement);
                    break;

                default:
                    LeftHandColumn.Children.Clear();
                    LeftHandColumn.Children.Add(frameworkElement);
                    break;
            }
        }
    }
}
