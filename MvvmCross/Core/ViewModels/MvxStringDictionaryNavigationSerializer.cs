// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.Parse.StringDictionary;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Core.ViewModels
{
    public class MvxStringDictionaryNavigationSerializer
        : IMvxNavigationSerializer
    {
        public IMvxTextSerializer Serializer => new MvxViewModelRequestCustomTextSerializer();
    }
}