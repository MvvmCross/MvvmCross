namespace Phone7.Fx.Imaging
{
    internal class HistogramResult
    {
            /// <summary>Averaged image brightness values over one scanned band</summary>
            public byte[] Histogram;
            /// <summary>Minimum brightness (darkest)</summary>
            public byte Min { get; set; }
            /// <summary>Maximum brightness (lightest)</summary>
            public byte Max { get; set; }

            public byte Threshold { get; set; }   // threshold brightness to detect change from "light" to "dark" color
            public float Lightnarrowbarwidth;// narrow bar width for light bars
            public float Darknarrowbarwidth;  // narrow bar width for dark bars
            public float Lightwiderbarwidth; // width of most common wider bar for light bars
        public float Darkwiderbarwidth;   // width of most common wider bar for dark bars

           // list of zones on the current band that might contain barcode data
        public BarcodeZone[] Zones { get; set; }
    }
}