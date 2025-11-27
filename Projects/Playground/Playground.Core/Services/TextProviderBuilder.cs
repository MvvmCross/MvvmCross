// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Plugin.JsonLocalization;

namespace Playground.Core.Services
{
    [RequiresUnreferencedCode("MvxTextProvider requires unreferenced code")]
    public class TextProviderBuilder : MvxTextProviderBuilder
    {
        public TextProviderBuilder() : base("Playground.Core", "Resources", new MvxEmbeddedJsonDictionaryTextProvider(false))
        {
        }

        protected override IDictionary<string, string> ResourceFiles
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "Text", "Text" }
                };
            }
        }
    }
}
