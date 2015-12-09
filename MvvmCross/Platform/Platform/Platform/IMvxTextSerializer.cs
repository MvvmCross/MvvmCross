// IMvxTextSerializer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Platform
{
    using System;

    public interface IMvxTextSerializer
    {
        T DeserializeObject<T>(string inputText);

        string SerializeObject(object toSerialise);

        object DeserializeObject(Type type, string inputText);
    }
}