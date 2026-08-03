using System;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using Playground.iOS.Controls;

namespace Playground.iOS.Bindings
{
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public class BinaryEditTargetBinding
        : MvxConvertingTargetBinding<BinaryEdit, int>
    {
        private readonly BinaryEdit _target;

        public BinaryEditTargetBinding(BinaryEdit target)
            : base(target)
        {
            _target = target;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _target.MyCountChanged += Target_CountChanged;
        }

        private void Target_CountChanged(object sender, EventArgs e)
        {
            if (_target == null)
            {
                return;
            }

            var count = _target.GetCount();
            FireValueChanged(count);
        }

        protected override void SetValueImpl(BinaryEdit target, int value)
        {
            _target.SetThat(value);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_target != null)
                {
                    _target.MyCountChanged -= Target_CountChanged;
                }
            }

            base.Dispose(isDisposing);
        }
    }
}
