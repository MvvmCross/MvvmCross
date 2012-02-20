using System.Reflection;
using Android.Text;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Android.Target
{
    public class MvxEditTextTextTargetBinding : MvxPropertyInfoTargetBinding<EditText>
    {        
        public MvxEditTextTextTargetBinding(object target, PropertyInfo targetPropertyInfo) 
            : base(target, targetPropertyInfo)
        {
            var editText = View;
            if (editText == null)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Error,"Error - EditText is null in MvxEditTextTextTargetBinding");
            }
            else
            {
                editText.AfterTextChanged += EditTextOnAfterTextChanged;
            }
        }

        private void EditTextOnAfterTextChanged(object sender, AfterTextChangedEventArgs afterTextChangedEventArgs)
        {
            FireValueChanged(View.Text);
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.TwoWay;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null)
                {
                    editText.AfterTextChanged -= EditTextOnAfterTextChanged;
                }
            }
        }
    }
}