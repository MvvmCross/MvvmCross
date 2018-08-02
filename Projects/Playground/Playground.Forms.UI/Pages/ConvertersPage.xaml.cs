using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels.Samples;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.UI.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConvertersPage : MvxContentPage<ConvertersViewModel>
	{
		public ConvertersPage ()
		{
			InitializeComponent ();
		}
	}
}
