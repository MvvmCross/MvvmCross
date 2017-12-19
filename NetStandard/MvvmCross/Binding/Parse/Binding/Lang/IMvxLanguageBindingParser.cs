// IMvxLanguageBindingParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Parse.Binding.Lang
{
    public interface IMvxLanguageBindingParser
        : IMvxBindingParser
    {
        string DefaultConverterName { get; set; }
        string DefaultTextSourceName { get; set; }
    }
}