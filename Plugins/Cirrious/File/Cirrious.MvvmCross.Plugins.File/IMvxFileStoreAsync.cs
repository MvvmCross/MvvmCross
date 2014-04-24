﻿// IMvxFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using System.Threading.Tasks;

#region using

using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Cirrious.MvvmCross.Plugins.File
{
	public interface IMvxFileStoreAsync

    {
		Task<TryResult<string>> TryReadTextFileAsync(string path);
		Task<TryResult<byte[]>> TryReadBinaryFileAsync(string path);
		Task<bool> TryReadBinaryFileAsync(string path, Func<Stream, Task<bool>> readMethod);
		Task WriteFileAsync(string path, string contents);
		Task WriteFileAsync(string path, IEnumerable<byte> contents);
		Task WriteFileAsync(string path, Func<Stream, Task> writeMethod);
    }
}