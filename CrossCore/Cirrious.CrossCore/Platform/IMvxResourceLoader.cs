// IMvxResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;

namespace Cirrious.CrossCore.Platform
{
    public interface IMvxResourceLoader
    {
        bool ResourceExists(string resourcePath);

        string GetTextResource(string resourcePath);

        void GetResourceStream(string resourcePath, Action<Stream> streamAction);
    }
}