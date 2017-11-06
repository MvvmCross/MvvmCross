using System;
using MvvmCross.Forms.Views.EventSource;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxBasePageAdapter
    {
        private readonly IMvxEventSourcePage _eventSource;

        protected Page View => _eventSource as Page;

        public MvxBasePageAdapter(IMvxEventSourcePage eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource - eventSource should not be null");

            if (!(eventSource is Page))
                throw new ArgumentException("eventSource - eventSource should be a Page");

            _eventSource = eventSource;
            _eventSource.AppearingCalled += HandleAppearingCalled;
            _eventSource.DisappearingCalled += HandleDisappearingCalled;
        }

        public virtual void HandleAppearingCalled(object sender, EventArgs e)
        {

        }

        public virtual void HandleDisappearingCalled(object sender, EventArgs e)
        {

        }
    }
}
