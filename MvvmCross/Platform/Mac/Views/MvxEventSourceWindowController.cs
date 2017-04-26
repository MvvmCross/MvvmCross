using System;
using AppKit;
using Foundation;
using MvvmCross.Platform.Core;

namespace MvvmCross.Platform.Mac.Views
{
    public class MvxEventSourceWindowController
        : NSWindowController
          , IMvxEventSourceWindowController
    {
        protected MvxEventSourceWindowController()
        {
            this.Initialize();
        }

        protected MvxEventSourceWindowController(IntPtr handle)
            : base(handle)
        {
            this.Initialize();
        }

        protected MvxEventSourceWindowController(NSCoder coder)
            : base(coder)
        {
            this.Initialize();
        }

        protected MvxEventSourceWindowController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.Initialize();
        }

        private void Initialize()
        {
        }

        public override void LoadWindow()
        {
            base.LoadWindow();
        }

        public override void WindowDidLoad()
        {
            base.WindowDidLoad();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                this.DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler ViewDidLoadCalled;

        public event EventHandler DisposeCalled;
    }
}
