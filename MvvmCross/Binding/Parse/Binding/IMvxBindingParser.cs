// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Parse.Binding
{
    public interface IMvxBindingParser
    {
        bool TryParseBindingDescription(string text, out MvxSerializableBindingDescription requestedDescription);

        bool TryParseBindingSpecification(string text, out MvxSerializableBindingSpecification requestedBindings);
    }
}