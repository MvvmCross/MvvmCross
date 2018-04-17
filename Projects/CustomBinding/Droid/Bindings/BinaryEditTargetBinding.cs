using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.TestProjects.CustomBinding.Droid.Controls;

namespace MvvmCross.TestProjects.CustomBinding.Droid.Bindings
{
    public class BinaryEditTargetBinding : MvxAndroidTargetBinding<BinaryEdit, int>
    {
        public BinaryEditTargetBinding(BinaryEdit target) : base(target)
        {
        }

        public override void SubscribeToEvents()
        {
            Target.MyCountChanged += TargetOnMyCountChanged;
        }

        private void TargetOnMyCountChanged(object sender, EventArgs eventArgs)
        {
            var target = Target;

            if (target == null)
                return;

            var value = target.GetCount();
            FireValueChanged(value);
        }

        protected override void SetValueImpl(BinaryEdit target, int value)
        {
            var binaryEdit = target;
            binaryEdit.SetThat(value);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = Target;
                if (target != null)
                {
                    target.MyCountChanged -= TargetOnMyCountChanged;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}