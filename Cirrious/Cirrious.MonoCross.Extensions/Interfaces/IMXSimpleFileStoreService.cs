#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="IMXSimpleFileStoreService.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

#region using

using System;
using System.Collections.Generic;

#endregion

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