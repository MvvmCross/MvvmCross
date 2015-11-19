// IMvxBindingParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Parse.Binding
{
    public interface IMvxBindingParser
    {
        bool TryParseBindingDescription(string text, out MvxSerializableBindingDescription requestedDescription);

        bool TryParseBindingSpecification(string text, out MvxSerializableBindingSpecification requestedBindings);
    }
}