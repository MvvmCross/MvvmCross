// MvxIosResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Plugins.ResourceLoader.iOS
{
    public class MvxIosResourceLoader
        : MvxResourceLoader

    {
        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
			if (!File.Exists(resourcePath))
			{
				throw new MvxException("Failed to read file {0}", resourcePath);
			}

			using (var fileStream = File.Open(resourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				streamAction?.Invoke(fileStream);
			}
        }
    }
}