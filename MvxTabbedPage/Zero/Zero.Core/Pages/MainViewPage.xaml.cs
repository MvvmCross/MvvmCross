using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Zero.Core.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainViewPage : MvxTabbedPage
    {
        public MainViewPage()
        {
            this.InitializeComponent();
            this.IsVisible = false;
            this.Title = "123";
        }
    }
}