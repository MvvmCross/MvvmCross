// IMvxBindingDescriptionParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Parse.Binding;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxBindingDescriptionParser
    {
        IEnumerable<MvxBindingDescription> Parse(string text);
        IEnumerable<MvxBindingDescription> LanguageParse(string text);

        MvxBindingDescription ParseSingle(string text);

        MvxBindingDescription SerializableBindingToBinding(string targetName,
                                                           MvxSerializableBindingDescription description);
    }
}