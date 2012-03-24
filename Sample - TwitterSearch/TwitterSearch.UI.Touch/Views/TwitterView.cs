using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Touch.Views
{
    public class TwitterView
        : MvxBindingTouchTableViewController<TwitterViewModel>
    {
        const string CellBindingText = @"
                        {
                           'TitleText':{'Path':'Author'},
                           'DetailText':{'Path':'Title'},
                           'HttpImageUrl':{'Path':'ProfileImageUrl'}
                        }";
		
		private UIActivityIndicatorView _activityView;
		
        public TwitterView(MvxShowViewModelRequest request)
            : base(request)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
			_activityView = new UIActivityIndicatorView(this.View.Frame);
			Add(_activityView);
			View.BringSubviewToFront(_activityView);
			
            var source = new MvxActionBasedBindableTableViewSource(
                                TableView, 
                                UITableViewCellStyle.Subtitle,
                                new NSString("Twitter"), 
                                CellBindingText,
								UITableViewCellAccessory.None);
			
			source.CellModifier = (cell) =>
				{
					cell.Image.DefaultImagePath = "Images/Icons/50_icon.png";
				};
			
            this.AddBindings(new Dictionary<object, string>()
		                         {
		                             {source, "{'ItemsSource':{'Path':'Tweets'}}"},
									 {_activityView, "{'Hidden':{'Path':'IsSearching','Converter':'InvertedVisibility'}}"},
		                         });
            TableView.Source = source;
			TableView.ReloadData();
        }
    }
}