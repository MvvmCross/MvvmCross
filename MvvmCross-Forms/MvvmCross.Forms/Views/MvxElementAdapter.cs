using System;
using MvvmCross.Forms.Views.EventSource;

namespace MvvmCross.Forms.Views
{
    public class MvxElementAdapter : MvxBaseElementAdapter
    {
        protected IMvxElement FormsView => View as IMvxElement;

        public MvxElementAdapter(IMvxEventSourceElement eventSource) : base(eventSource)
        {
            if (!(eventSource is IMvxElement))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxElement");
        }

        public override void HandleBindingContextChangedCalled(object sender, EventArgs e)
        {
            FormsView.OnBindingContextChanged();
            base.HandleBindingContextChangedCalled(sender, e);
        }
    }
}
