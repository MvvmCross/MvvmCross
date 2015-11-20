// MvxStringDictionaryNavigationSerializer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Parse.StringDictionary;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxStringDictionaryNavigationSerializer
        : IMvxNavigationSerializer
    {
        public IMvxTextSerializer Serializer => new MvxViewModelRequestCustomTextSerializer();
    }
}