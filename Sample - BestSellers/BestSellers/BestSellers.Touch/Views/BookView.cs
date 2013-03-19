using System;
using System.Drawing;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using BestSellers.ViewModels;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace BestSellers.Touch.Views
{
	public partial class BookView : MvxViewController
	{
        private MvxDynamicImageHelper<UIImage> _imageHelper;

		public new BookViewModel ViewModel {
			get { return base.ViewModel as BookViewModel; }
			set { base.ViewModel = value; }
		}

		public BookView () : base ("BookView", null)
		{
			_imageHelper = new MvxDynamicImageHelper<UIImage>();
			_imageHelper.ImageChanged += HandleImageHelperImageChanged;
		}

		void HandleImageHelperImageChanged (object sender, MvxValueEventArgs<UIImage> e)
		{
			if (BookImage != null)
			{
				BookImage.Image = e.Value;
			}			
		}
				
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
						
			this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { TitleLabel, "Text Book.Title" },
                        { AuthorLabel, "Text Book.Author" },
                        { DescriptionLabel, "Text Book.Description" },
                        { _imageHelper, "ImageUrl Book.AmazonImageUrl" },
                        { ActivityIndicator, "Hidden IsLoading,Converter=InvertedVisibility" },
                    });
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();					
			ReleaseDesignerOutlets ();
			_imageHelper.ImageChanged -= HandleImageHelperImageChanged;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

