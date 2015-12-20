// MvxStringDictionaryNavigationSerializer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using MvvmCross.Core.Parse.StringDictionary;
    using MvvmCross.Platform.Platform;

    public class MvxStringDictionaryNavigationSerializer
        : IMvxNavigationSerializer
    {
        public IMvxTextSerializer Serializer => new MvxViewModelRequestCustomTextSerializer();
    }
}