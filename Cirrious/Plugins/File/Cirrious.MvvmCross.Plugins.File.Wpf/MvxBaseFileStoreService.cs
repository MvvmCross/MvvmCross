#region Copyright
// <copyright file="MvxBaseFileStoreService.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Platform.Diagnostics;

#warning THIS FILE NOW COPIED NOT SHARED

namespace Cirrious.MvvmCross.Plugins.File.Wpf
{
#if !NETFX_CORE
    public abstract class MvxBaseFileStoreService 
        : IMvxSimpleFileStoreService
    {
        #region IMvxSimpleFileStoreService Members

        public bool Exists(string path)
        {
            var fullPath = FullPath(path);
            return System.IO.File.Exists(fullPath); 
        }

        public string PathCombine(string items0, string items1)
        {
            return Path.Combine(items0, items1);
        }

        public void EnsureFolderExists(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }

        public IEnumerable<string> GetFilesIn(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return Directory.GetFiles(fullPath);
        }

        public void DeleteFile(string filePath)
        {
            var fullPath = FullPath(filePath);
            System.IO.File.Delete(fullPath);
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
                                                               if (binaryReader.Read(memoryBuffer, 0,
                                                                                     memoryBuffer.Length) !=
                                                                   memoryBuffer.Length)
                                                                   return false; // TODO - do more here?

                                                               result = memoryBuffer;
                                                               return true;
                                                           }
                                                       });
            contents = result;
            return toReturn;
        }

        public bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod)
        {
            return TryReadFileCommon(path, readMethod);
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
                var fullFrom = FullPath(from);
                var fullTo = FullPath(to);

                if (!System.IO.File.Exists(fullFrom))
                    return false;

                if (System.IO.File.Exists(fullTo))
                {
                    if (deleteExistingTo)
                        System.IO.File.Delete(fullTo);
                    else
                        return false;
                }

                System.IO.File.Move(fullFrom, fullTo);
                return true;
            }
            //catch (ThreadAbortException)
            //{
            //    throw;
            //}
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file move {0} : {1} : {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        #endregion

        protected abstract string FullPath(string path);

        private void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            var fullPath = FullPath(path);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            using (var fileStream = System.IO.File.OpenWrite(fullPath))
                streamAction(fileStream);
        }

        private bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            var fullPath = FullPath(path);
            if (!System.IO.File.Exists(fullPath))
            {
                return false;
            }

            using (var fileStream = System.IO.File.OpenRead(fullPath))
            {
                return streamAction(fileStream);
            }
        }
    }
#endif
}
