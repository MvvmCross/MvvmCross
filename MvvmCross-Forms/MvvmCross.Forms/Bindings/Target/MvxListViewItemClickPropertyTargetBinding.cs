using System;
using System.Reflection;
using System.Windows.Input;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Forms.Views;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings.Target
{
    public class MvxListViewItemClickPropertyTargetBinding : MvxPropertyInfoTargetBinding<MvxListView>
    {
        private bool _subscribed;

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public MvxListViewItemClickPropertyTargetBinding(object target, PropertyInfo targetPropertyInfo) : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as MvxListView;
            if (view == null) return;

            view.ItemClick = (ICommand)value;
        }

        public override void SubscribeToEvents()
        {
            var myView = View;
            if (myView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, $"Error - MyView is null in {nameof(MvxListViewItemClickPropertyTargetBinding)}");
                return;
            }

            _subscribed = true;
            myView.ItemTapped += HandleMyPropertyChanged;
        }

        private void HandleMyPropertyChanged(object sender, EventArgs e)
        {
            var view = sender as MvxListView;
            var args = e as ItemTappedEventArgs;
            if (args == null) return;

            view.ItemClick.Execute(args.Item);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (isDisposing)
            {
                var myView = View;
                if (myView != null && _subscribed)
                {
                    myView.ItemTapped -= HandleMyPropertyChanged;
                    _subscribed = false;
                }
            }
        }
    }
}
