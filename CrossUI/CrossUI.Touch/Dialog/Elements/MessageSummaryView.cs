// MessageSummaryView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using CrossUI.Touch.Dialog.Utilities;
using System;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class MessageSummaryView : UIView
    {
        private static readonly UIFont SenderFont = UIFont.BoldSystemFontOfSize(19);
        private static readonly UIFont SubjectFont = UIFont.SystemFontOfSize(14);
        private static readonly UIFont TextFont = UIFont.SystemFontOfSize(13);
        private static readonly UIFont CountFont = UIFont.BoldSystemFontOfSize(13);
        private string _sender;

        public string Sender
        {
            get { return _sender; }
            set
            {
                _sender = value;
                SetNeedsDisplay();
            }
        }

        private string _body;

        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                SetNeedsDisplay();
            }
        }

        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                SetNeedsDisplay();
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                SetNeedsDisplay();
            }
        }

        private bool _newFlag;

        public bool NewFlag
        {
            get { return _newFlag; }
            set
            {
                _newFlag = value;
                SetNeedsDisplay();
            }
        }

        private int _messageCount;

        public int MessageCount
        {
            get { return _messageCount; }
            set
            {
                _messageCount = value;
                SetNeedsDisplay();
            }
        }

        private static readonly CGGradient Gradient;

        static MessageSummaryView()
        {
            using (var colorspace = CGColorSpace.CreateDeviceRGB())
            {
                Gradient = new CGGradient(colorspace,
                                          new nfloat[] {/* first */ .52f, .69f, .96f, 1, /* second */ .12f, .31f, .67f, 1 },
                                          null); //new nfloat [] { 0, 1 });
            }
        }

        public MessageSummaryView()
        {
#warning Virtual method call in constructor
            BackgroundColor = UIColor.White;
        }

        public void Update(string sender, string body, string subject, DateTime date, bool newFlag, int messageCount)
        {
            Sender = sender;
            Body = body;
            Subject = subject;
            Date = date;
            NewFlag = newFlag;
            MessageCount = messageCount;
        }

        public override void Draw(CGRect rect)
        {
            const int padright = 21;
            var ctx = UIGraphics.GetCurrentContext();
            nfloat boxWidth;
            CGSize ssize;

            if (MessageCount > 0)
            {
                var ms = MessageCount.ToString();
                ssize = ms.StringSize(CountFont);
                boxWidth = NMath.Min(22 + ssize.Width, 18);
                var crect = new CGRect(Bounds.Width - 20 - boxWidth, 32, boxWidth, 16);

                UIColor.Gray.SetFill();
                GraphicsUtil.FillRoundedRect(ctx, crect, 3);
                UIColor.White.SetColor();
                crect.X += 5;
                ms.DrawString(crect, CountFont);

                boxWidth += padright;
            }
            else
                boxWidth = 0;

            UIColor.FromRGB(36, 112, 216).SetColor();
            var diff = DateTime.Now - Date;
            string label;
            if (DateTime.Now.Day == Date.Day)
                label = Date.ToShortTimeString();
            else if (diff <= TimeSpan.FromHours(24))
                label = "Yesterday".GetText();
            else if (diff < TimeSpan.FromDays(6))
                label = Date.ToString("dddd");
            else
                label = Date.ToShortDateString();
            ssize = label.StringSize(SubjectFont);
            nfloat dateSize = ssize.Width + padright + 5;
            label.DrawString(new CGRect(Bounds.Width - dateSize, 6, dateSize, 14), SubjectFont,
                             UILineBreakMode.Clip, UITextAlignment.Left);

            const int offset = 33;
            nfloat bw = Bounds.Width - offset;

            UIColor.Black.SetColor();
            Sender.DrawString(new CGPoint(offset, 2), bw - dateSize, SenderFont, UILineBreakMode.TailTruncation);
            Subject.DrawString(new CGPoint(offset, 23), bw - offset - boxWidth, SubjectFont,
                       UILineBreakMode.TailTruncation);

            //UIColor.Black.SetFill ();
            //ctx.FillRect (new CGRect(offset, 40, bw - boxWidth, 34));
            UIColor.Gray.SetColor();
            Body.DrawString(new CGRect(offset, 40, bw - boxWidth, 34), TextFont, UILineBreakMode.TailTruncation,
                       UITextAlignment.Left);

            if (NewFlag)
            {
                ctx.SaveState();
                ctx.AddEllipseInRect(new CGRect(10, 32, 12, 12));
                ctx.Clip();
                ctx.DrawLinearGradient(Gradient, new CGPoint(10, 32), new CGPoint(22, 44),
                                       CGGradientDrawingOptions.DrawsAfterEndLocation);
                ctx.RestoreState();
            }

#if WANT_SHADOWS
            ctx.SaveState();
            UIColor.FromRGB(78, 122, 198).SetStroke();
            ctx.SetShadow(new CGSize(1, 1), 3);
            ctx.StrokeEllipseInRect(new CGRect(10, 32, 12, 12));
            ctx.RestoreState();
#endif
        }
    }
}