using System;
using MvvmCross.Forms.Views.EventSource;

namespace MvvmCross.Forms.Views
{
    public class MvxPageAdapter : MvxBasePageAdapter
    {
        protected IMvxPage FormsView => View as IMvxPage;

        public MvxPageAdapter(IMvxEventSourcePage eventSource) : base(eventSource)
        {
            if (!(eventSource is IMvxPage))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxPage");
        }

        public override void HandleAppearingCalled(object sender, EventArgs e)
        {
            FormsView.OnViewAppearing();
            base.HandleAppearingCalled(sender, e);
        }
    }
}