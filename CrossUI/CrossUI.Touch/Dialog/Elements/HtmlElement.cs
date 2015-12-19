// HtmlElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Touch.Dialog.Utilities;
using Foundation;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    /// <summary>
    ///  Used to display a cell that will launch a web browser when selected.
    /// </summary>
    public class HtmlElement : Element
    {
        private static readonly NSString Hkey = new NSString("HtmlElement");
        private NSUrl _nsUrl;
        private UIWebView _web;

        public HtmlElement()
            : this("", "")
        {
        }

        public HtmlElement(string caption, string url)
            : base(caption)
        {
            Url = url;
        }

        public HtmlElement(string caption, NSUrl url)
            : base(caption)
        {
            _nsUrl = url;
        }

        protected override NSString CellKey => Hkey;

        public string Url
        {
            get { return _nsUrl.ToString(); }
            set { _nsUrl = new NSUrl(value); }
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey) ?? new UITableViewCell(UITableViewCellStyle.Default, CellKey)
            {
                SelectionStyle = UITableViewCellSelectionStyle.Blue
            };
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            return cell;
        }

        private static bool NetworkActivity
        {
            set { UIApplication.SharedApplication.NetworkActivityIndicatorVisible = value; }
        }

        // We use this class to dispose the web control when it is not
        // in use, as it could be a bit of a pig, and we do not want to
        // wait for the GC to kick-in.
        private class WebViewController : UIViewController
        {
            private readonly HtmlElement _container;

            public WebViewController(HtmlElement container)
            {
                this._container = container;
            }

            public override void ViewWillDisappear(bool animated)
            {
                base.ViewWillDisappear(animated);
                NetworkActivity = false;
                if (_container._web == null)
                    return;

                _container._web.StopLoading();
                _container._web.Dispose();
                _container._web = null;
            }

            public bool Autorotate { get; set; }

            public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
            {
                return Autorotate;
            }
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            var vc = new WebViewController(this)
            {
                Autorotate = dvc.Autorotate
            };

            _web = new UIWebView(UIScreen.MainScreen.Bounds)
            {
                BackgroundColor = UIColor.White,
                ScalesPageToFit = true,
                AutoresizingMask = UIViewAutoresizing.All
            };
            _web.LoadStarted += delegate
            {
                NetworkActivity = true;
                var indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
                vc.NavigationItem.RightBarButtonItem = new UIBarButtonItem(indicator);
                indicator.StartAnimating();
            };
            _web.LoadFinished += delegate
            {
                NetworkActivity = false;
                vc.NavigationItem.RightBarButtonItem = null;
            };
            _web.LoadError += (webview, args) =>
            {
                NetworkActivity = false;
                vc.NavigationItem.RightBarButtonItem = null;
                _web?.LoadHtmlString(
                    $"<html><center><font size=+5 color='red'>{"An error occurred:".GetText()}:<br>{args.Error.LocalizedDescription}</font></center></html>", null);
            };
            vc.NavigationItem.Title = Caption;

            vc.View.AutosizesSubviews = true;
            vc.View.AddSubview(_web);

            dvc.ActivateController(vc);
            _web.LoadRequest(NSUrlRequest.FromUrl(_nsUrl));
        }
    }
}