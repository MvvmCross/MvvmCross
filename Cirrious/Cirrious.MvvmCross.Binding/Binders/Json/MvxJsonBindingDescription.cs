using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Binders.Json
{
    public class MvxJsonBindingDescription
    {
#warning if monotouch then this might need a preserve!
        public string Path { get; set; }
        public string Converter { get; set; }
        public string ConverterParameter { get; set; }
        public string FallbackValue { get; set; }
        public MvxBindingMode Mode { get; set; }
    }
}