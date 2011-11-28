#region Copyright
// <copyright file="MvxIsolatedStorageFileStoreService.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using Cirrious.MvvmCross.Interfaces.Services;

#endregion

namespace Cirrious.MvvmCross.WindowsPhone.Services
{
    public class MvxIsolatedStorageFileStoreService : IMvxSimpleFileStoreService
    {
        #region IMvxSimpleFileStoreService Members

        public bool TryReadTextFile(string path, out string contents)
        {
            string result = null;
            var toReturn = TryReadFileCommon(path, (stream) =>
                                                       {
                                                           using (var streamReader = new StreamReader(stream))
                                                           {
                                                               result = streamReader.ReadToEnd();
                                                           }
                                                           return true;
                                                       });
            contents = result;
            return toReturn;
        }

        public bool TryReadBinaryFile(string path, out Byte[] contents)
        {
            throw new Exception("Untested method");
            Byte[] result = null;
            var toReturn = TryReadFileCommon(path, (stream) =>
                                                       {
                                                           using (var binaryReader = new BinaryReader(stream))
                                                           {
                                                               var memoryBuffer = new byte[stream.Length];
                                                               if (
                                                                   binaryReader.Read(memoryBuffer, 0,
                                                                                     memoryBuffer.Length) !=
                                                                   memoryBuffer.Length)
                                                                   return false; // TODO - do more here?

                                                               return true;
                                                           }
                                                       });
            contents = result;
            return toReturn;
        }

        public void WriteFile(string path, string contents)
        {
            WriteFileCommon(path, (stream) =>
                                      {
                                          using (var sw = new StreamWriter(stream))
                                          {
                                              sw.Write(contents);
                                              sw.Flush();
                                          }
                                      });
        }

        public void WriteFile(string path, IEnumerable<Byte> contents)
        {
            throw new Exception("Untested method");
            WriteFileCommon(path, (stream) =>
                                      {
                                          using (var binaryWriter = new BinaryWriter(stream))
                                          {
                                              binaryWriter.Write(contents.ToArray());
                                              binaryWriter.Flush();
                                          }
                                      });
        }

        #endregion

        private static void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fileStream = new IsolatedStorageFileStream(path, FileMode.Create, isf))
                    streamAction(fileStream);
            }
        }

        private static bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.FileExists(path))
                {
                    return false;
                }

                using (var fileStream = isf.OpenFile(path, FileMode.Open))
                {
                    return streamAction(fileStream);
                }
            }
        }
    }
}