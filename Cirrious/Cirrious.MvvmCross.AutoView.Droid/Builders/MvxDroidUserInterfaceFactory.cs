using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using CrossUI.Core.Builder;
using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxDroidUserInterfaceFactory 
        : IMvxUserInterfaceFactory
          , IMvxServiceConsumer 
    {
        public TResult Build<TBuildable, TResult>(IMvxAutoView view, KeyedDescription description)
        {
            var bindingActivity = view as IMvxDroidBindingContext;
            if (bindingActivity == null)
                throw new MvxException("View passed to MvxDroidUserInterfaceFactory must be an IMvxDroidBindingContext - type {0}", view.GetType().Name);
            
            var registry = this.GetService<IBuilderRegistry>();
            var builder = new MvxDroidUserInterfaceBuilder(bindingActivity, view.ViewModel, registry);
            var root = (TResult)builder.Build(typeof(TBuildable), description);
            return root;
        }
    }
}