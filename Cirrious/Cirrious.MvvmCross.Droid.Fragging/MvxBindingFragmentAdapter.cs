using System;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public class MvxBindingFragmentAdapter
        : MvxBaseFragmentAdapter
    {
        protected IMvxAndroidFragmentView FragmentView
        {
            get { return base.Fragment as IMvxAndroidFragmentView; }
        }

        public MvxBindingFragmentAdapter(IMvxEventSourceFragment eventSource) 
            : base(eventSource)
        {
            if (!(eventSource is IMvxAndroidFragmentView))
                throw new ArgumentException("eventSource must be an IMvxAndroidFragmentView");
        }

        protected override void HandleCreateViewCalled(object sender, MvxValueEventArgs<MvxCreateViewParameters> mvxValueEventArgs)
        {
            if (FragmentView.BindingContext == null)
            {
                FragmentView.BindingContext = new MvxBindingContext(Fragment.Activity, FragmentView);
            }
        }

        protected override void HandleDestroyViewCalled(object sender, EventArgs eventArgs)
        {
            if (FragmentView.BindingContext != null)
            {
                FragmentView.BindingContext.ClearAllBindings();
            }
            base.HandleDestroyViewCalled(sender, eventArgs);
        }
    }
}