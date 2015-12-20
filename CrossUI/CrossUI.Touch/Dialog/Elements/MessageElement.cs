// MessageElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using System;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class MessageElement : Element, IElementSizing
    {
        private static readonly NSString MKey = new NSString("MessageElement");

        private void UpdateActiveCell<TCell>(Action<TCell> action)
            where TCell : UITableViewCell
        {
            var cell = GetActiveCell();
            var typedCell = cell as TCell;
            if (typedCell == null)
                return;
            action(typedCell);
        }

        private string _sender;

        public string Sender
        {
            get { return _sender; }
            set
            {
                _sender = value;
                UpdateActiveCell<MessageCell>((cell) => cell.View.Sender = _sender);
            }
        }

        private string _body;

        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                UpdateActiveCell<MessageCell>((cell) => cell.View.Body = _body);
            }
        }

        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                UpdateActiveCell<MessageCell>((cell) => cell.View.Subject = _subject);
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                UpdateActiveCell<MessageCell>((cell) => cell.View.Date = _date);
            }
        }

        private bool _newFlag;

        public bool NewFlag
        {
            get { return _newFlag; }
            set
            {
                _newFlag = value;
                UpdateActiveCell<MessageCell>((cell) => cell.View.NewFlag = _newFlag);
            }
        }

        private int _messageCount;

        public int MessageCount
        {
            get { return _messageCount; }
            set
            {
                _messageCount = value;
                UpdateActiveCell<MessageCell>((cell) => cell.View.MessageCount = _messageCount);
            }
        }

        private sealed class MessageCell : UITableViewCell
        {
            private MessageSummaryView _view;

            public MessageSummaryView View
            {
                get { return _view; }
                private set { _view = value; }
            }

            public MessageCell()
                : base(UITableViewCellStyle.Default, MKey)
            {
                View = new MessageSummaryView();
                ContentView.Add(View);
                Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }

            public void Update(MessageElement me)
            {
                View.Update(me.Sender, me.Body, me.Subject, me.Date, me.NewFlag, me.MessageCount);
            }

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();
                View.Frame = ContentView.Bounds;
                View.SetNeedsDisplay();
            }
        }

        public MessageElement()
            : base("")
        {
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(MKey) as MessageCell ?? new MessageCell();
            cell.Update(this);
            return cell;
        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return 78;
        }

        //public event Action<DialogViewController, UITableView, NSIndexPath> Tapped;

        //public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
        //{
        //    if (Tapped != null)
        //        Tapped (dvc, tableView, path);
        //}
    }
}