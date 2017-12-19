using System;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCross.Forms.Bindings
{
    public abstract class MvxBaseBindExtension : IMarkupExtension
    {
        protected IMvxLog Log = Mvx.Resolve<IMvxLogProvider>().GetLogFor<MvxBaseBindExtension>();

        public MvxBindingMode Mode { get; set; } = MvxBindingMode.Default;
        public string Converter { get; set; }
        public string ConverterParameter { get; set; }
        public string FallbackValue { get; set; }

        public IProvideValueTarget Target { get; private set; }
        public BindableObject BindableObj { get; private set; }
        public BindableProperty BindableProp { get; private set; }
        public string PropertyName { get; private set;  }

        public abstract object ProvideValue(IServiceProvider serviceProvider);

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            Target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            BindableObj = Target.TargetObject as BindableObject;
            BindableProp = Target.TargetProperty as BindableProperty;

            if (Target.TargetProperty is BindableProperty bp)
            {
                BindableProp = bp;
                PropertyName = bp.PropertyName;
            }
            else if (Target.TargetProperty is PropertyInfo pi)
                PropertyName = pi.Name;

            return ProvideValue(serviceProvider);
        }
    }
}
