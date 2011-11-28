using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using Phone7.Fx.Extensions;

namespace Phone7.Fx.Imaging
{
    internal class Parser
    {
        private  int scanY = 0;
        private  int scanX = 0;

        // General
        private const int GAPFACTOR = 48;        // width of quiet zone compared to narrow bar
        private const int MINNARROWBARCOUNT = 4; // minimum occurence of a narrow bar width
        
        // code39
        private const float WIDEFACTOR = 2.0f; // minimum width of wide bar compared to narrow bar
        private const int MINPATTERNLENGTH = 10; // length of one barcode digit + gap


      

        StringBuilder sbCode39Pattern;
        StringBuilder sbEANPattern;

        public Parser()
        {
            sbCode39Pattern = new StringBuilder();
            sbEANPattern = new StringBuilder();
        }

        public  string ReadBarcodes(WriteableBitmap bmp, int start, int end, ScanDirection direction, BarcodeType types)
        {
            string barCodes = "|"; // will hold return values

            // To find a horizontal barcode, find the vertical histogram to find individual barcodes, 
            // then get the vertical histogram to decode each
            HistogramResult vertHist = VerticalHistogram(bmp, start, end, direction);

            // Get the light/dark bar patterns.
            // GetBarPatterns returns the bar pattern in 2 formats: 
            //
            //   sbCode39Pattern: for Code39 (only distinguishes narrow bars "n" and wide bars "w")
            //   sbEANPattern: for EAN (distinguishes bar widths 1, 2, 3, 4 and L/G-code)
            //
           

            GetBarPatterns(vertHist);

            // We now have a barcode in terms of narrow & wide bars... Parse it!
            if ((sbCode39Pattern.Length > 0) || (sbEANPattern.Length > 0))
            {
                for (int iPass = 0; iPass < 2; iPass++)
                {
                    if ((types & BarcodeType.Code39) != BarcodeType.None) // if caller wanted Code39
                    {
                        string sCode39 = Code39Parser.ParseCode(sbCode39Pattern);
                        if (sCode39.Length > 0)
                            barCodes += sCode39 + "|";
                    }
                    if ((types & BarcodeType.EAN) != BarcodeType.None) // if caller wanted EAN
                    {
                        string sEAN = EANParser.ParseCode(sbEANPattern);
                        if (sEAN.Length > 0)
                            barCodes += sEAN + "|";
                    }
                    if ((types & BarcodeType.Code128) != BarcodeType.None) // if caller wanted Code128
                    {
                        // Note: Code128 uses same bar width measurement data as EAN
                        string sCode128 = Code128Parser.ParseCode(sbEANPattern);
                        if (sCode128.Length > 0)
                            barCodes += sCode128 + "|";
                    }

                    // Reverse the bar pattern arrays to read again in the mirror direction
                    if (iPass == 0)
                    {
                        sbCode39Pattern = Helper.Reverse(sbCode39Pattern);
                        sbEANPattern = Helper.Reverse(sbEANPattern);
                    }
                }
            }

            // Return pipe-separated list of found barcodes, if any
            if (barCodes.Length > 2)
                return barCodes.Substring(1, barCodes.Length - 2);
            return string.Empty;
        }

        private  void GetBarPatterns( HistogramResult hist)
        {
            // Initialize return data
           

            if (hist.Zones != null) // if barcode zones were found along the scan line
            {
                for (int iZone = 0; iZone < hist.Zones.Length; iZone++)
                {
                    // Recalculate bar width distribution if more than one zone is present, it could differ per zone
                    if (hist.Zones.Length > 1)
                        GetBarWidthDistribution(hist, hist.Zones[iZone].Start, hist.Zones[iZone].End);

                    // Check the calculated narrow bar widths. If they are very different, the pattern is
                    // unlikely to be a bar code
                    if (ValidBars(hist))
                    {
                        // add gap separator to output patterns
                        sbCode39Pattern.Append("|");
                        sbEANPattern.Append("|");

                        // Variables needed to check for
                        int iBarStart = 0;
                        bool bDarkBar = (hist.Histogram[0] <= hist.Threshold);

                        // Find the narrow and wide bars
                        for (int i = 1; i < hist.Histogram.Length; ++i)
                        {
                            bool bDark = (hist.Histogram[i] <= hist.Threshold);
                            if (bDark != bDarkBar)
                            {
                                int iBarWidth = i - iBarStart;
                                float fNarrowBarWidth = bDarkBar ? hist.Darknarrowbarwidth : hist.Lightnarrowbarwidth;
                                float fWiderBarWidth = bDarkBar ? hist.Darkwiderbarwidth : hist.Lightwiderbarwidth;
                                if (IsWideBar(iBarWidth, fNarrowBarWidth, fWiderBarWidth))
                                {
                                    // The bar was wider than the narrow bar width, it's a wide bar or a gap
                                    if (iBarWidth > GAPFACTOR * fNarrowBarWidth)
                                    {
                                        sbCode39Pattern.Append("|");
                                        sbEANPattern.Append("|");
                                    }
                                    else
                                    {
                                        sbCode39Pattern.Append("w");
                                        AppendEAN(sbEANPattern, iBarWidth, fNarrowBarWidth);
                                    }
                                }
                                else
                                {
                                    // The bar is a narrow bar
                                    sbCode39Pattern.Append("n");
                                    AppendEAN(sbEANPattern, iBarWidth, fNarrowBarWidth);
                                }
                                bDarkBar = bDark;
                                iBarStart = i;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Used by GetBarPatterns to derive bar character from bar width.
        /// </summary>
        /// <param name="sbEAN">Output pattern</param>
        /// <param name="nBarWidth">Measured bar width in pixels</param>
        /// <param name="fNarrowBarWidth">Narrow bar width in pixels</param>
        private static void AppendEAN(StringBuilder sbEAN, int nBarWidth, float fNarrowBarWidth)
        {
            int nEAN = (int)Math.Round((double)nBarWidth / fNarrowBarWidth);
            if (nEAN == 5) nEAN = 4; // bar width could be slightly off due to distortion
            if (nEAN < 10)
                sbEAN.Append(nEAN.ToString());
            else
                sbEAN.Append("|");
        }

        /// <summary>
        /// Returns true if the bar appears to be "wide".
        /// </summary>
        /// <param name="iBarWidth">measured bar width in pixels</param>
        /// <param name="fNarrowBarWidth">average narrow bar width</param>
        /// <param name="fWiderBarWidth">average width of next wider bar</param>
        /// <returns></returns>
        private static bool IsWideBar(int iBarWidth, float fNarrowBarWidth, float fWiderBarWidth)
        {
            if (fNarrowBarWidth < 4.0)
                return (iBarWidth > WIDEFACTOR * fNarrowBarWidth);
            return (iBarWidth >= fWiderBarWidth) || ((fWiderBarWidth - iBarWidth) < (iBarWidth - fNarrowBarWidth));
        }

        /// <summary>
        /// Checks if dark and light narrow bar widths are in agreement.
        /// </summary>
        /// <param name="hist">barcode data</param>
        /// <returns>true if barcode data is valid</returns>
        private static bool ValidBars( HistogramResult hist)
        {
            float fCompNarrowBarWidths = hist.Lightnarrowbarwidth / hist.Darknarrowbarwidth;
            float fCompWiderBarWidths = hist.Lightwiderbarwidth / hist.Darkwiderbarwidth;
            return ((fCompNarrowBarWidths >= 0.5) && (fCompNarrowBarWidths <= 2.0)
                 && (fCompWiderBarWidths >= 0.5) && (fCompWiderBarWidths <= 2.0)
                 && (hist.Darkwiderbarwidth / hist.Darknarrowbarwidth >= 1.5)
                 && (hist.Lightwiderbarwidth / hist.Lightnarrowbarwidth >= 1.5));
        }

       

        private  HistogramResult VerticalHistogram(WriteableBitmap bmp, int start, int end, ScanDirection direction)
        {
            int xMax;
            int yMax;

            if (direction == ScanDirection.Horizontal)
            {
                xMax = bmp.PixelHeight;
                yMax = end - start;
            }
            else
            {
                xMax = bmp.PixelWidth;
                yMax = end - start;
            }


            // Create the return value
            byte[] histResult = new byte[xMax + 2]; // add 2 to simulate light-colored background pixels at sart and end of scanline
            int[] vertSum = new int[xMax];


            if (direction == ScanDirection.Horizontal)
            {
                int xEnd = (end - start) + scanX;

                for (; scanX < xEnd; scanX++)
                {
                    for (int y = 0; y < bmp.PixelHeight; y++)
                    {
                        var pixel = bmp.GetPixel(scanX, y);
                        vertSum[y] += (pixel.R + pixel.B + pixel.G);
                    }
                }
            }
            else
            {
                int yEnd = (end - start) + scanY;
                for (; scanY < yEnd; scanY++)
                {
                    for (int x = 0; x < bmp.PixelWidth; x++)
                    {
                        var pixel = bmp.GetPixel(x, scanY);
                        vertSum[x] += (pixel.R + pixel.B + pixel.G);
                    }
                }
            }



            // Now get the average of the row by dividing the pixel by num pixels
            int iDivider = end - start;
            // if (pf != PixelFormat.Format1bppIndexed)
            iDivider *= 3;

            byte maxValue = byte.MinValue; // Start the max value at zero
            byte minValue = byte.MaxValue; // Start the min value at the absolute maximum

            for (int i = 1; i <= xMax; i++) // note: intentionally skips first pixel in histResult
            {
                histResult[i] = (byte)(vertSum[i - 1] / iDivider);
                //Save the max value for later
                if (histResult[i] > maxValue) maxValue = histResult[i];
                // Save the min value for later
                if (histResult[i] < minValue) minValue = histResult[i];
            }

            // Set first and last pixel to "white", i.e., maximum intensity
            histResult[0] = maxValue;
            histResult[xMax + 1] = maxValue;

            HistogramResult retVal = new HistogramResult
                                         {
                                             Histogram = histResult,
                                             Max = maxValue,
                                             Min = minValue,
                                             Threshold = (byte) (minValue + ((maxValue - minValue) >> 1))
                                         };

            // Now we have the brightness distribution along the scan band, try to find the distribution of bar widths.

            GetBarWidthDistribution(retVal, 0, retVal.Histogram.Length);

            // Now that we know the narrow bar width, lets look for barcode zones.
            // The image could have more than one barcode in the same band, with 
            // different bar widths.
            FindBarcodeZones(retVal);
            return retVal;
        }

        /// <summary>
        /// Gets the bar width distribution and calculates narrow bar width over the specified
        /// range of the histogramResult. A histogramResult could have multiple ranges, separated 
        /// by quiet zones.
        /// </summary>
        /// <param name="hist">histogramResult data</param>
        /// <param name="start">start coordinate to be considered</param>
        /// <param name="end">end coordinate + 1</param>
        private static void GetBarWidthDistribution(HistogramResult hist, int start, int end)
        {
            Dictionary<int, int> lightBars = new Dictionary<int, int>();
            Dictionary<int, int> darkBars = new Dictionary<int, int>();
            bool darkBar = (hist.Histogram[start] <= hist.Threshold);
            int barStart = 0;
            for (int i = start + 1; i < end; i++)
            {
                bool bDark = (hist.Histogram[i] <= hist.Threshold);
                if (bDark != darkBar)
                {
                    int iBarWidth = i - barStart;
                    if (darkBar)
                    {
                        if (!darkBars.ContainsKey(iBarWidth))
                            darkBars.Add(iBarWidth, 1);
                        else
                            darkBars[iBarWidth] = (int)darkBars[iBarWidth] + 1;
                    }
                    else
                    {
                        if (!lightBars.ContainsKey(iBarWidth))
                            lightBars.Add(iBarWidth, 1);
                        else
                            lightBars[iBarWidth] = (int)lightBars[iBarWidth] + 1;
                    }
                    darkBar = bDark;
                    barStart = i;
                }
            }

            // Now get the most common bar widths
            CalcNarrowBarWidth(lightBars, out hist.Lightnarrowbarwidth, out hist.Lightwiderbarwidth);
            CalcNarrowBarWidth(darkBars, out hist.Darknarrowbarwidth, out hist.Darkwiderbarwidth);
        }

        private static void CalcNarrowBarWidth(Dictionary<int, int> barWidths, out float fNarrowBarWidth, out float fWiderBarWidth)
        {
            fNarrowBarWidth = 1.0f;
            fWiderBarWidth = 2.0f;
            if (barWidths.Count > 1) // we expect at least two different bar widths in supported barcodes
            {
                int[] aiWidths = new int[barWidths.Count];
                int[] aiCounts = new int[barWidths.Count];
                int i = 0;
                foreach (int iKey in barWidths.Keys)
                {
                    aiWidths[i] = iKey;
                    aiCounts[i] = barWidths[iKey];
                    i++;
                }
                Array.Sort(aiWidths, aiCounts);

                // walk from lowest to highest width. The narrowest bar should occur at least 4 times
                fNarrowBarWidth = aiWidths[0];
                fWiderBarWidth = WIDEFACTOR * fNarrowBarWidth;
                for (i = 0; i < aiCounts.Length; i++)
                {
                    if (aiCounts[i] >= MINNARROWBARCOUNT)
                    {
                        fNarrowBarWidth = aiWidths[i];
                        if (fNarrowBarWidth < 3)
                            fWiderBarWidth = WIDEFACTOR * fNarrowBarWidth;
                        else
                        {
                            // if the width is not singular, look for the most common width in the neighbourhood
                            float fCount;
                            FindPeakWidth(i, ref aiWidths, ref aiCounts, out fNarrowBarWidth, out fCount);
                            fWiderBarWidth = WIDEFACTOR * fNarrowBarWidth;

                            if (fNarrowBarWidth >= 6)
                            {
                                // ... and for the next wider common bar width if the barcode is fairly large
                                float fMaxCount = 0.0f;
                                for (int j = i + 1; j < aiCounts.Length; j++)
                                {
                                    float fNextWidth, fNextCount;
                                    FindPeakWidth(j, ref aiWidths, ref aiCounts, out fNextWidth, out fNextCount);
                                    if (fNextWidth/fNarrowBarWidth <= 1.5)
                                        continue;
                                    if (fNextCount > fMaxCount)
                                    {
                                        fWiderBarWidth = fNextWidth;
                                        fMaxCount = fNextCount;
                                    }
                                    else
                                        break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        static void FindPeakWidth(int i, ref int[] aiWidths, ref int[] aiCounts, out float fWidth, out float fCount)
        {
            fWidth = 0.0f;
            fCount = 0.0f;
            int iSamples = 0;
            for (int j = i - 1; j <= i + 1; j++)
            {
                if ((j >= 0) && (j < aiWidths.Length) && (Math.Abs(aiWidths[j] - aiWidths[i]) == Math.Abs(j - i)))
                {
                    iSamples++;
                    fCount += aiCounts[j];
                    fWidth += aiWidths[j] * aiCounts[j];
                }
            }
            fWidth /= fCount;
            fCount /= iSamples;
        }

        /// <summary>
        /// FindBarcodeZones looks for barcode zones in the current band. 
        /// We look for white space that is more than GAPFACTOR * narrowbarwidth
        /// separating two zones. For narrowbarwidth we take the maximum of the 
        /// dark and light narrow bar width.
        /// </summary>
        /// <param name="hist">Data for current image band</param>
        private static void FindBarcodeZones(HistogramResult hist)
        {
            if (!ValidBars(hist))
                hist.Zones = null;
           
                List<BarcodeZone> barcodeZones = new List<BarcodeZone>();
                bool darkBar = (hist.Histogram[0] <= hist.Threshold);
                int barStart = 0;
                int zoneStart = -1;
                int zoneEnd = -1;
                float quietZoneWidth = GAPFACTOR * (hist.Darknarrowbarwidth + hist.Lightnarrowbarwidth) / 2;
                float minZoneWidth = quietZoneWidth;

                for (int i = 1; i < hist.Histogram.Length; i++)
                {
                    bool dark = (hist.Histogram[i] <= hist.Threshold);
                    if (dark != darkBar)
                    {
                        int iBarWidth = i - barStart;
                        if (!darkBar) // This ends a light area
                        {
                            if ((zoneStart == -1) || (iBarWidth > quietZoneWidth))
                            {
                                // the light area can be seen as a quiet zone
                                zoneEnd = i - (iBarWidth >> 1);

                                // Check if the active zone is big enough to contain a barcode
                                if ((zoneStart >= 0) && (zoneEnd > zoneStart + minZoneWidth))
                                {
                                    // record the barcode zone that ended in the detected quiet zone ...
                                    BarcodeZone bz = new BarcodeZone {Start = zoneStart, End = zoneEnd};
                                    barcodeZones.Add(bz);

                                    // .. and start a new barcode zone
                                    zoneStart = zoneEnd;
                                }
                                if (zoneStart == -1)
                                    zoneStart = zoneEnd; // first zone starts here
                            }
                        }
                        darkBar = dark;
                        barStart = i;
                    }
                }
                if (zoneStart >= 0)
                {
                    BarcodeZone bz = new BarcodeZone {Start = zoneStart, End = hist.Histogram.Length};
                    barcodeZones.Add(bz);
                }
                if (barcodeZones.Count > 0)
                    hist.Zones = barcodeZones.ToArray();
                else
                    hist.Zones = null;
            
        }

        #region Code39-specific
        
        #endregion

    

    

    }
}