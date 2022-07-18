// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MvvmCross.Binding.Parse.Binding
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
