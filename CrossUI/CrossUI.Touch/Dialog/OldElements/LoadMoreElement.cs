// LoadMoreElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using CrossUI.Touch.Dialog.Elements;
using Foundation;
using System;
using UIKit;

namespace CrossUI.Touch.Dialog.OldElements
{
    public class LoadMoreElement : Element, IElementSizing
    {
        private static readonly NSString key = new NSString("LoadMoreElement");
        public string NormalCaption { get; set; }
        public string LoadingCaption { get; set; }
        public UIColor TextColor { get; set; }
        public UIColor BackgroundColor { get; set; }

        public event Action<LoadMoreElement> LoadMoreTapped = null;

        public UIFont Font;
        public nfloat? Height;
        private UITextAlignment alignment = UITextAlignment.Center;
        private bool animating;

        public LoadMoreElement() : base("")
        {
        }

        public LoadMoreElement(string normalCaption, string loadingCaption, Action<LoadMoreElement> tapped)
            : this(normalCaption, loadingCaption, tapped, UIFont.BoldSystemFontOfSize(16), UIColor.Black)
        {
        }

        public LoadMoreElement(string normalCaption, string loadingCaption, Action<LoadMoreElement> tapped, UIFont font,
                               UIColor textColor) : base("")
        {
            NormalCaption = normalCaption;
            LoadingCaption = loadingCaption;
            LoadMoreTapped += tapped;
            Font = font;
            TextColor = textColor;
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(key);
            UIActivityIndicatorView activityIndicator;
            UILabel caption;

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, key);

                activityIndicator = new UIActivityIndicatorView
                {
                    ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray,
                    Tag = 1
                };
                caption = new UILabel
                {
                    AdjustsFontSizeToFitWidth = false,
                    Tag = 2
                };
                cell.ContentView.AddSubview(caption);
                cell.ContentView.AddSubview(activityIndicator);
            }
            else
            {
                activityIndicator = cell.ContentView.ViewWithTag(1) as UIActivityIndicatorView;
                caption = cell.ContentView.ViewWithTag(2) as UILabel;
            }
            if (Animating)
            {
                if (caption != null) caption.Text = LoadingCaption;
                if (activityIndicator != null)
                {
                    activityIndicator.Hidden = false;
                    activityIndicator.StartAnimating();
                }
            }
            else
            {
                if (caption != null) caption.Text = NormalCaption;
                if (activityIndicator != null)
                {
                    activityIndicator.Hidden = true;
                    activityIndicator.StopAnimating();
                }
            }
            if (BackgroundColor != null)
            {
                cell.ContentView.BackgroundColor = BackgroundColor ?? UIColor.Clear;
            }
            else
            {
                cell.ContentView.BackgroundColor = null;
            }
            if (caption != null)
            {
                caption.BackgroundColor = UIColor.Clear;
                caption.TextColor = TextColor ?? UIColor.Black;
                caption.Font = Font ?? UIFont.BoldSystemFontOfSize(16);
                caption.TextAlignment = Alignment;
                Layout(cell, activityIndicator, caption);
            }
            return cell;
        }

        public bool Animating
        {
            get { return animating; }
            set
            {
                if (animating == value)
                    return;
                animating = value;
                var cell = GetActiveCell();
                if (cell == null)
                    return;
                var activityIndicator = cell.ContentView.ViewWithTag(1) as UIActivityIndicatorView;
                var caption = cell.ContentView.ViewWithTag(2) as UILabel;
                if (value)
                {
                    if (caption != null) caption.Text = LoadingCaption;
                    if (activityIndicator != null)
                    {
                        activityIndicator.Hidden = false;
                        activityIndicator.StartAnimating();
                    }
                }
                else
                {
                    if (activityIndicator != null)
                    {
                        activityIndicator.StopAnimating();
                        activityIndicator.Hidden = true;
                    }
                    if (caption != null) caption.Text = NormalCaption;
                }
                Layout(cell, activityIndicator, caption);
            }
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            tableView.DeselectRow(path, true);

            if (Animating)
                return;

            if (LoadMoreTapped != null)
            {
                Animating = true;
                LoadMoreTapped(this);
            }
        }

        private CGSize GetTextSize(string text)
        {
            return new NSString(text).StringSize(Font, UIScreen.MainScreen.Bounds.Width, UILineBreakMode.TailTruncation);
        }

        private const int pad = 10;
        private const int isize = 20;

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return Height ?? GetTextSize(Animating ? LoadingCaption : NormalCaption).Height + 2 * pad;
        }

        private void Layout(UITableViewCell cell, UIActivityIndicatorView activityIndicator, UILabel caption)
        {
            var sbounds = cell.ContentView.Bounds;

            var size = GetTextSize(Animating ? LoadingCaption : NormalCaption);

            if (!activityIndicator.Hidden)
                activityIndicator.Frame = new CGRect((sbounds.Width - size.Width) / 2 - isize * 2, pad, isize, isize);

            caption.Frame = new CGRect(10, pad, sbounds.Width - 20, size.Height);
        }

        public UITextAlignment Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        public UITableViewCellAccessory Accessory { get; set; }
    }
}