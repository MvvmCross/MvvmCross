using System;
using System.Collections.Generic;

namespace Phone7.Fx.Imaging
{
    public class BarcodeEventArgs:EventArgs
    {
        /// <summary>
        /// Gets the list of the barcode
        /// </summary>
        /// <value>The found.</value>
        public List<string> Found { get; internal set; }
    }
}