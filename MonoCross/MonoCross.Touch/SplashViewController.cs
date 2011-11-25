using System;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
    /// <summary>
    /// Class to display the initial view when still warming up
    /// </summary>
	internal class SplashViewController: UIViewController
	{
		UIImageView _imageView;
		
		public SplashViewController (string imageFile)
		{
			UIImage image = null;
			if (!String.IsNullOrEmpty(imageFile))
				image = UIImage.FromFile(imageFile);

			if (image != null)
				_imageView = new UIImageView(image);
			else
				_imageView = new UIImageView();
			_imageView.ContentMode = UIViewContentMode.Center;
			_imageView.BackgroundColor = UIColor.White;

			this.View = _imageView;
		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);
			
			/*
			UIImageView view = this.View as UIImageView;
			
			if (toInterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || 
			    toInterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
				view.Image = UIImage.FromFile("Images/Launch-Landscape.png");
			else
				view.Image = UIImage.FromFile("Images/Launch-Portrait.png");
			*/
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

