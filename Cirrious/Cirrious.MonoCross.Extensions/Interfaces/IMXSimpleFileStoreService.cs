using System;
using System.Collections.Generic;

namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXSimpleFileStoreService
    {
        bool TryReadTextFile(string path, out string contents);
        bool TryReadBinaryFile(string path, out Byte[] contents);
        void WriteFile(string path, string contents);
        void WriteFile(string path, IEnumerable<Byte> contents);
    }
}