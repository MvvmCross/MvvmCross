using System;

namespace Phone7.Fx.Imaging
{
    /// <summary>
    /// Used to specify what barcode type(s) to detect.
    /// </summary>
    [Flags]
    public enum BarcodeType
    {
        /// <summary>Not specified</summary>
        None = 0,
        /// <summary>Code39</summary>
        Code39 = 1,
        /// <summary>EAN/UPC</summary>
        EAN = 2,
        /// <summary>Code128</summary>
        Code128 = 4,
        /// <summary>Use BarcodeType.All for all supported types</summary>
        All = Code39 | EAN | Code128

        // Note: Extend this enum with new types numbered as 8, 16, 32 ... ,
        //       so that we can use bitwise logic: All = Code39 | EAN | <your favorite type here> | ...
    }
}