// IMvxFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;

namespace MvvmCross.Plugins.File
{
    public interface IMvxFileStore
    {
        bool TryReadTextFile(string path, out string contents);

        bool TryReadBinaryFile(string path, out Byte[] contents);

        bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod);

        void WriteFile(string path, string contents);

        void WriteFile(string path, IEnumerable<Byte> contents);

        void WriteFile(string path, Action<Stream> writeMethod);

		bool TryMove(string from, string to, bool overwrite);

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
    }
}