using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using Cirrious.MonoCross.Extensions.Interfaces;

namespace Cirrious.MonoCross.Extensions.Android.Android
{
    public class MXFileStoreService : IMXSimpleFileStoreService
    {
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
                                            if (binaryReader.Read(memoryBuffer, 0, memoryBuffer.Length) != memoryBuffer.Length)
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

        private static void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            using (var fileStream = File.OpenWrite(path))
                streamAction(fileStream);
        }

        private static bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            using (var fileStream = File.OpenRead(path))
            {
                return streamAction(fileStream);
            }
        }
    }
}
