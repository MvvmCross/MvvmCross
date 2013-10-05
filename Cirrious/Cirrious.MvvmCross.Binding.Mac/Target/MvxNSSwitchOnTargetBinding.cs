// MvxUISwitchOnTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
    public class MvxNSSwitchOnTargetBinding : MvxPropertyInfoTargetBinding<NSButton>
    {
		private class Feedback : NSObject
		{
			private MvxNSSwitchOnTargetBinding _owner;
			public Feedback (MvxNSSwitchOnTargetBinding owner, NSButton button)
			{
				_owner = owner;
				//button.
				button.Action = new MonoMac.ObjCRuntime.Selector ("checkBoxAction:");
			}

			[Export("checkBoxAction:")]
			private void checkBoxAction()
			{
				_owner.checkBoxAction ();
			}
		}


		Feedback _feedback;
        public MvxNSSwitchOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var checkBox = View;
            if (checkBox == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSButton is null in MvxNSSwitchOnTargetBinding");
            }
            else
            {	
				checkBox.Activated += (object sender, System.EventArgs e) =>  checkBoxAction();
				//_feedback = new Feedback (this, checkBox);
           }
        }

		private void checkBoxAction()
		{
			var view = View;
			if (view == null)
				return;
			FireValueChanged(view.State == NSCellStateValue.On);
		}

		protected override object MakeSafeValue (object value)
		{
			if (value is bool) {
				if ((bool)value) {
					return (NSCellStateValue.On);
				} else {
					return (NSCellStateValue.Off);
				}
			}
			return base.MakeSafeValue (value);
		}

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null)
                {
//                    view.ValueChanged -= HandleValueChanged;
                }
            }
        }
    }
}