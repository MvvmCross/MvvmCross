// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MvvmCross.Plugin.File
{
    public interface IMvxFileStore
    {
        ValueTask<(bool result, string contents)> TryReadTextFile(string path);

        ValueTask<(bool result, byte[] contents)> TryReadBinaryFile(string path);

        ValueTask<bool> TryReadBinaryFile(string path, Func<Stream, bool> readMethod);

        void WriteFile(string path, string contents);

        void WriteFile(string path, IEnumerable<byte> contents);

        void WriteFile(string path, Action<Stream> writeMethod);

		ValueTask<bool> TryMove(string from, string to, bool overwrite);

        ValueTask<bool> TryCopy(string from, string to, bool overwrite);

        ValueTask<bool> Exists(string path);

        ValueTask<bool> FolderExists(string folderPath);

        string PathCombine(string items0, string items1);

        string NativePath(string path);

        ValueTask EnsureFolderExists(string folderPath);

        ValueTask<IEnumerable<string>> GetFilesIn(string folderPath);

        ValueTask<IEnumerable<string>> GetFoldersIn(string folderPath);

        void DeleteFile(string path);

        ValueTask DeleteFolder(string folderPath, bool recursive);

        ValueTask<Stream> OpenRead(string path);

        ValueTask<Stream> OpenWrite(string path);

        ValueTask<long> GetSize(string path);

        ValueTask<DateTime> GetLastWriteTimeUtc(string path);
    }
}
