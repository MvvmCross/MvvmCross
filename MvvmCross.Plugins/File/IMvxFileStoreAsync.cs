// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmCross.Plugin.File
{
    /// <summary>
    ///     Warning: this interface not implemented on all platforms currently
    /// </summary>
    public interface IMvxFileStoreAsync
    {
        ValueTask<TryResult<string>> TryReadTextFileAsync(string path);

        ValueTask<TryResult<string>> TryReadTextFileAsync(string path, CancellationToken cancellationToken);

        ValueTask<TryResult<byte[]>> TryReadBinaryFileAsync(string path);

        ValueTask<TryResult<byte[]>> TryReadBinaryFileAsync(string path, CancellationToken cancellationToken);

        ValueTask<bool> TryReadBinaryFileAsync(string path, Func<Stream, ValueTask<bool>> readMethod);

        ValueTask<bool> TryReadBinaryFileAsync(string path, Func<Stream, CancellationToken, ValueTask<bool>> readMethod,
            CancellationToken cancellationToken);

        ValueTask WriteFileAsync(string path, string contents);

        ValueTask WriteFileAsync(string path, string contents, CancellationToken cancellationToken);

        ValueTask WriteFileAsync(string path, IEnumerable<byte> contents);

        ValueTask WriteFileAsync(string path, IEnumerable<byte> contents, CancellationToken cancellationToken);

        ValueTask WriteFileAsync(string path, Func<Stream, ValueTask> writeMethod);

        ValueTask WriteFileAsync(string path, Func<Stream, CancellationToken, ValueTask> writeMethod,
            CancellationToken cancellationToken);
    }
}
