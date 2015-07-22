using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Example.Pages
{
    public partial class FirstPage : ContentPage
    {
        public FirstPage()
        {
            InitializeComponent();

            // Xamarin OnPlatform does not yet support W81
            if (Device.OS == TargetPlatform.Windows)
                Padding = new Thickness(Padding.Left, this.Padding.Top, this.Padding.Right, 95);
        }
    }
}
