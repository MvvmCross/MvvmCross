// IMvxTextSerializer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Platform
{
    public interface IMvxTextSerializer
    {
        T DeserializeObject<T>(string inputText);

        string SerializeObject(object toSerialise);

        object DeserializeObject(Type type, string inputText);
    }
}