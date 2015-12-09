// IMvxResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Platform
{
    using System;
    using System.IO;

    public interface IMvxResourceLoader
    {
        bool ResourceExists(string resourcePath);

        string GetTextResource(string resourcePath);

        void GetResourceStream(string resourcePath, Action<Stream> streamAction);
    }
}