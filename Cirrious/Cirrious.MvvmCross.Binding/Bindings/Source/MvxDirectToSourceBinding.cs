using System;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public class MvxDirectToSourceBinding : MvxBaseSourceBinding
    {
        public MvxDirectToSourceBinding(object source) 
            : base(source)
        {

        }

        public override void SetValue(object value)
        {
            MvxBindingTrace.Trace(MvxBindingTraceLevel.Warning,
                                  "ToSource binding is not available for direct pathed source bindings");
        }

        public override Type SourceType
        {
            get
            {
                return Source == null ? typeof (object) : Source.GetType(); }
        }

        public override bool TryGetValue(out object value)
        {
            value = Source;
            return true;
        }
    }
}