using System;
using System.Drawing;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Utilities;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class MessageSummaryView : UIView 
    {
        static readonly UIFont SenderFont = UIFont.BoldSystemFontOfSize (19);
        static readonly UIFont SubjectFont = UIFont.SystemFontOfSize (14);
        static readonly UIFont TextFont = UIFont.SystemFontOfSize (13);
        static readonly UIFont CountFont = UIFont.BoldSystemFontOfSize (13);
        private string _sender;
        public string Sender
        {
            get { return _sender; }
            set
            {
                _sender = value; SetNeedsDisplay();
            }
        }

        private string _body;
        public string Body
        {
            get { return _body; }
            set
            {
                _body = value; SetNeedsDisplay();
            }
        }

        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value; SetNeedsDisplay();
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value; SetNeedsDisplay();
            }
        }

        private bool _newFlag;
        public bool NewFlag
        {
            get { return _newFlag; }
            set
            {
                _newFlag = value; SetNeedsDisplay();
            }
        }

        private int _messageCount;
        public int MessageCount
        {
            get { return _messageCount; }
            set
            {
                _messageCount = value; SetNeedsDisplay();
            }
        }

        static readonly CGGradient Gradient;
        
        static MessageSummaryView ()
        {
            using (var colorspace = CGColorSpace.CreateDeviceRGB ()){
                Gradient = new CGGradient (colorspace, new float [] { /* first */ .52f, .69f, .96f, 1, /* second */ .12f, .31f, .67f, 1 }, null); //new float [] { 0, 1 });
            }
        }
        
        public MessageSummaryView ()
        {
#warning Virtual method call in constructor
            BackgroundColor = UIColor.White;
        }
        
        public void Update (string sender, string body, string subject, DateTime date, bool newFlag, int messageCount)
        {
            Sender = sender;
            Body = body;
            Subject = subject;
            Date = date;
            NewFlag = newFlag;
            MessageCount = messageCount;
        }
        
        public override void Draw (RectangleF rect)
        {
            const int padright = 21;
            var ctx = UIGraphics.GetCurrentContext ();
            float boxWidth;
            SizeF ssize;
            
            if (MessageCount > 0){
                var ms = MessageCount.ToString ();
                ssize = StringSize (ms, CountFont);
                boxWidth = Math.Min (22 + ssize.Width, 18);
                var crect = new RectangleF (Bounds.Width-20-boxWidth, 32, boxWidth, 16);
                
                UIColor.Gray.SetFill ();
                GraphicsUtil.FillRoundedRect (ctx, crect, 3);
                UIColor.White.SetColor ();
                crect.X += 5;
                DrawString (ms, crect, CountFont);
                
                boxWidth += padright;
            } else
                boxWidth = 0;
            
            UIColor.FromRGB (36, 112, 216).SetColor ();
            var diff = DateTime.Now - Date;
            string label;
            if (DateTime.Now.Day == Date.Day)
                label = Date.ToShortTimeString ();
            else if (diff <= TimeSpan.FromHours (24))
                label = "Yesterday".GetText ();
            else if (diff < TimeSpan.FromDays (6))
                label = Date.ToString ("dddd");
            else
                label = Date.ToShortDateString ();
            ssize = StringSize (label, SubjectFont);
            float dateSize = ssize.Width + padright + 5;
            DrawString (label, new RectangleF (Bounds.Width-dateSize, 6, dateSize, 14), SubjectFont, UILineBreakMode.Clip, UITextAlignment.Left);
            
            const int offset = 33;
            float bw = Bounds.Width-offset;
            
            UIColor.Black.SetColor ();
            DrawString (Sender, new PointF (offset, 2), bw-dateSize, SenderFont, UILineBreakMode.TailTruncation);
            DrawString (Subject, new PointF (offset, 23), bw-offset-boxWidth, SubjectFont, UILineBreakMode.TailTruncation);
            
            //UIColor.Black.SetFill ();
            //ctx.FillRect (new RectangleF (offset, 40, bw-boxWidth, 34));
            UIColor.Gray.SetColor ();
            DrawString (Body, new RectangleF (offset, 40, bw-boxWidth, 34), TextFont, UILineBreakMode.TailTruncation, UITextAlignment.Left);
            
            if (NewFlag){
                ctx.SaveState ();
                ctx.AddEllipseInRect (new RectangleF (10, 32, 12, 12));
                ctx.Clip ();
                ctx.DrawLinearGradient (Gradient, new PointF (10, 32), new PointF (22, 44), CGGradientDrawingOptions.DrawsAfterEndLocation);
                ctx.RestoreState ();
            }
            
#if WANT_SHADOWS
            ctx.SaveState ();
            UIColor.FromRGB (78, 122, 198).SetStroke ();
            ctx.SetShadow (new SizeF (1, 1), 3);
            ctx.StrokeEllipseInRect (new RectangleF (10, 32, 12, 12));
            ctx.RestoreState ();
#endif
        }
    }
}