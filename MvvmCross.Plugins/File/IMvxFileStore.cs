// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace MvvmCross.Plugin.File
{
    public interface IMvxFileStore
    {
        bool TryReadTextFile(string path, out string contents);

        bool TryReadBinaryFile(string path, out byte[] contents);

        bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod);

        void WriteFile(string path, string contents);

        void WriteFile(string path, IEnumerable<byte> contents);

        void WriteFile(string path, Action<Stream> writeMethod);

		bool TryMove(string from, string to, bool overwrite);

        bool TryCopy(string from, string to, bool overwrite);

        bool Exists(string path);

        bool FolderExists(string folderPath);

        string PathCombine(string items0, string items1);

        string NativePath(string path);

        void EnsureFolderExists(string folderPath);

        IEnumerable<string> GetFilesIn(string folderPath);

        IEnumerable<string> GetFoldersIn(string folderPath);

        void DeleteFile(string path);

        void DeleteFolder(string folderPath, bool recursive);

        Stream OpenRead(string path);

        Stream OpenWrite(string path);

        long GetSize(string path);

        DateTime GetLastWriteTimeUtc(string path);
    }
}
