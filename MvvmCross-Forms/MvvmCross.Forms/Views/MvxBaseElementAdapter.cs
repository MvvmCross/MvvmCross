using System;
using MvvmCross.Forms.Views.EventSource;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxBaseElementAdapter
    {
        private readonly IMvxEventSourceElement _eventSource;

        protected Element View => _eventSource as Element;

        public MvxBaseElementAdapter(IMvxEventSourceElement eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource - eventSource should not be null");

            if (!(eventSource is Element))
                throw new ArgumentException("eventSource - eventSource should be an Element");

            _eventSource = eventSource;
            _eventSource.BindingContextChangedCalled += HandleBindingContextChangedCalled;
            _eventSource.ParentSetCalled += HandleParentSetCalled;
        }

        public virtual void HandleBindingContextChangedCalled(object sender, EventArgs e)
        {

        }

        public virtual void HandleParentSetCalled(object sender, EventArgs e)
        {

        }
    }
}