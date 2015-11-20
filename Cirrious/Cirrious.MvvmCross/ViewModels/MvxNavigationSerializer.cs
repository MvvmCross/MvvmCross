// MvxNavigationSerializer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxNavigationSerializer
        : IMvxNavigationSerializer
    {
        public MvxNavigationSerializer(IMvxTextSerializer serializer)
        {
            Serializer = serializer;
        }

        public IMvxTextSerializer Serializer { get; private set; }
    }

    public class MvxNavigationSerializer<T>
        : MvxNavigationSerializer
        where T : class, IMvxTextSerializer
    {
        public MvxNavigationSerializer()
            : base(Mvx.Resolve<T>())
        {
        }
    }
}