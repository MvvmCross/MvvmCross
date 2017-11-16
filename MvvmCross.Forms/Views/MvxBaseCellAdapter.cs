using System;
using MvvmCross.Forms.Views.EventSource;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxBaseCellAdapter
    {
        private readonly IMvxEventSourceCell _eventSource;

        protected Cell View => _eventSource as Cell;

        public MvxBaseCellAdapter(IMvxEventSourceCell eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource - eventSource should not be null");

            if (!(eventSource is Cell))
                throw new ArgumentException("eventSource - eventSource should be a Cell");

            _eventSource = eventSource;
            _eventSource.AppearingCalled += HandleAppearingCalled;
            _eventSource.DisappearingCalled += HandleDisappearingCalled;
            _eventSource.TappedCalled += HandleTappedCalled;
            _eventSource.BindingContextChangedCalled += HandleBindingContextChangedCalled;
            _eventSource.ParentSetCalled += HandleParentSetCalled;
        }

        public virtual void HandleAppearingCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleDisappearingCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleTappedCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleBindingContextChangedCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleParentSetCalled(object sender, EventArgs e)
        {
        }
    }
}