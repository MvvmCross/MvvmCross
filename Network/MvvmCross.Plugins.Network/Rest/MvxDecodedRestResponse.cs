// MvxDecodedRestResponse.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxDecodedRestResponse<T>
        : MvxRestResponse
    {
        public T Result { get; set; }
    }
}