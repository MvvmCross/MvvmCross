#region Copyright
// <copyright file="MvxIsolatedStorageFileStoreService.cs" company="Cirrious">
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
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Platform.Diagnostics;

#endregion

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    public class MvxIsolatedStorageFileStoreService 
        : IMvxSimpleFileStoreService
    {
        #region IMvxSimpleFileStoreService Members

        public bool Exists(string path)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isf.FileExists(path);
            }
        }

        public void EnsureFolderExists(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(folderPath))
                    return;
                isf.CreateDirectory(folderPath);
            }
        }

        public IEnumerable<string> GetFilesIn(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var path = folderPath + "/*";
                return isf.GetFileNames(folderPath).Select(x => folderPath + "/" + x).ToArray();
            }
        }

        public void DeleteFile(string path)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                isf.DeleteFile(path);
            }
        }

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

        public bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod)
        {
            var toReturn = TryReadFileCommon(path, readMethod);
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
            WriteFileCommon(path, (stream) =>
                                      {
                                          using (var binaryWriter = new BinaryWriter(stream))
                                          {
                                              binaryWriter.Write(contents.ToArray());
                                              binaryWriter.Flush();
                                          }
                                      });
        }

        public void WriteFile(string path, Action<Stream> writeMethod)
        {
            WriteFileCommon(path, writeMethod);
        }

        public bool TryMove(string from, string to, bool deleteExistingTo)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.FileExists(from))
                        return false;

                    if (isf.FileExists(to))
                    {
                        if (deleteExistingTo)
                            isf.DeleteFile(to);
                        else
                            return false;
                    }

                    isf.MoveFile(from, to);
                    return true;
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error masked during file move {0} : {1} : {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        #endregion

        private static void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var fileStream = new IsolatedStorageFileStream(path, FileMode.Create, isf))
                        streamAction(fileStream);
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        private static bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            try
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
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return false;
            }
        }
    }
}