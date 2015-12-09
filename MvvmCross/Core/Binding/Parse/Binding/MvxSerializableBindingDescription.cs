// MvxSerializableBindingDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Parse.Binding
{
    public class MvxSerializableBindingDescription
    {
        public string Converter { get; set; }
        public object ConverterParameter { get; set; }
        public object FallbackValue { get; set; }
        public MvxBindingMode Mode { get; set; }
        public IList<MvxSerializableBindingDescription> Sources { get; set; }
        public string Function { get; set; }
        public object Literal { get; set; }
        public string Path { get; set; }
    }
}