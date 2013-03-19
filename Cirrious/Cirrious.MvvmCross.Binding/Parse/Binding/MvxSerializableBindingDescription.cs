// MvxSerializableBindingDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Parse.Binding
{
#if MONOTOUCH
    [MonoTouch.Foundation.Preserve(AllMembers = true)]
#endif

    public class MvxSerializableBindingDescription
    {
        public string Path { get; set; }
        public string Converter { get; set; }
        public object ConverterParameter { get; set; }
        public object FallbackValue { get; set; }
        public MvxBindingMode Mode { get; set; }
    }
}