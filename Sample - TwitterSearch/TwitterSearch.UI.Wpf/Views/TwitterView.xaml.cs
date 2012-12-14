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
using Cirrious.MvvmCross.Wpf.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for TwitterView.xaml
    /// </summary>
    [Region("Detail")] 
    public partial class TwitterView : MvxWpfView
    {
        public TwitterView()
        {
            InitializeComponent();
        }

        public new TwitterViewModel ViewModel
        {
            get { return base.ViewModel as TwitterViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
