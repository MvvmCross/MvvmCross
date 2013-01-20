// IMvxBindingDescriptionParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;

namespace Cirrious.MvvmCross.Binding.Interfaces.Binders
{
    public interface IMvxBindingDescriptionParser
    {
        IEnumerable<MvxBindingDescription> Parse(string text);
        MvxBindingDescription ParseSingle(string text);

        MvxBindingDescription SerializableBindingToBinding(string targetName,
                                                           MvxSerializableBindingDescription description);
    }
}