#region Copyright
// <copyright file="IMvxSimpleFileStoreService.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Cirrious.MvvmCross.Interfaces.Platform
{
    public interface IMvxSimpleFileStoreService
    {
        bool TryReadTextFile(string path, out string contents);
        bool TryReadBinaryFile(string path, out Byte[] contents);
        bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod);
        void WriteFile(string path, string contents);
        void WriteFile(string path, IEnumerable<Byte> contents);
        void WriteFile(string path, Action<Stream> writeMethod);
        bool TryMove(string from, string to, bool deleteExistingTo);
        bool Exists(string path);

        void EnsureFolderExists(string folderPath);
        IEnumerable<string> GetFilesIn(string folderPath);
        void DeleteFile(string path);
    }
}


#warning Reefactor needed