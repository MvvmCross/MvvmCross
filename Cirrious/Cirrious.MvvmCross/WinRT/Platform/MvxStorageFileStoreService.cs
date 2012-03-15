#region Copyright
// <copyright file="MvxStorageFileStoreService.cs" company="Cirrious">
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
using System.Linq;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Windows.Storage;

#endregion

namespace Cirrious.MvvmCross.WinRT.Platform
{
    public class MvxStorageFileStoreService 
        : IMvxSimpleFileStoreService
    {
        #region IMvxSimpleFileStoreService Members

        public bool Exists(string path)
        {
            throw new NotImplementedException();
        }

        public void EnsureFolderExists(string folderPath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFilesIn(string folderPath)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public bool TryReadTextFile(string path, out string contents)
        {
            throw new NotImplementedException();
        }

        public bool TryReadBinaryFile(string path, out Byte[] contents)
        {
            throw new NotImplementedException();
        }

        public bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod)
        {
            throw new NotImplementedException();
        }

        public void WriteFile(string path, string contents)
        {
            throw new NotImplementedException();
        }

        public void WriteFile(string path, IEnumerable<Byte> contents)
        {
            throw new NotImplementedException();
        }

        public void WriteFile(string path, Action<Stream> writeMethod)
        {
            throw new NotImplementedException();
        }

        public bool TryMove(string from, string to, bool deleteExistingTo)
        {
            throw new NotImplementedException();
        }

        #endregion

        private static void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            throw new NotImplementedException();
        }

        private static bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            throw new NotImplementedException();
        }
    }
}