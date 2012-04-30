using System;
using System.Text;

namespace Phone7.Fx.Imaging
{
    internal class Code128Parser
    {
        // Code128
        private const int CODE128START = 103;    // Startcodes have index >= 103
        private const int CODE128STOP = 106; // Stopcode has index 106
        private const int CODE128C = 99;  // Switch to code page C
        private const int CODE128B = 100; // Switch to code page B
        private const int CODE128A = 101; // Switch to code page A

        #region Code128-specific
        /// <summary>
        /// Parses Code128 barcodes.
        /// </summary>
        /// <param name="sbPattern">Input pattern, should contain characters
        /// "1" .. "4" to indicate valid bar widths.</param>
        /// <returns>Pipe-separated list of barcodes, empty string if none were detected</returns>
        internal static string ParseCode(StringBuilder pattern)
        {
            StringBuilder code128Data = new StringBuilder(32);
            string code128Code = string.Empty;
            uint checkSum = 0;
            int codes = 0;
            int pos = 0;
            char codePage = 'B';
            while (pos <= pattern.Length - 6)
            {
                int result = ParseCode128Pattern(pattern.ToString(pos, 6), ref code128Code, ref checkSum, ref codePage, ref codes);
                switch (result)
                {
                    case -1: // unrecognized pattern
                        pos++;
                        break;
                    case -2: // stop condition, but failed to recognize barcode
                        pos += 7;
                        break;
                    case CODE128STOP:
                        pos += 7;
                        if (code128Code.Length > 0)
                        {
                            if (code128Data.Length > 0)
                                code128Data.Append("|");
                            code128Data.Append(code128Code);
                        }
                        break;
                    default:
                        pos += 6;
                        break;
                }
            }
            return code128Data.ToString();
        }

        /// <summary>
        /// Parses the Code128 pattern for one barcode character.
        /// </summary>
        /// <param name="pattern">Pattern to be parsed, should be 6 characters</param>
        /// <param name="result">Resulting barcode up to current character</param>
        /// <param name="checkSum">Checksum up to current character</param>
        /// <param name="codePage">Current code page</param>
        /// <param name="codes">Count of barcode characters already parsed (needed for checksum)</param>
        /// <returns>
        /// CODE128STOP: end of barcode detected, barcode recognized.
        ///          -2: end of barcode, recognition failed.
        ///          -1: unrecognized pattern.
        ///       other: code 128 character index
        /// </returns>
        private static int ParseCode128Pattern(string pattern, ref string result, ref uint checkSum, ref char codePage, ref int codes)
        {
            string[] Code128 = 
                {"212222", "222122", "222221", "121223", "121322", "131222", 
                 "122213", "122312", "132212", "221213", "221312", "231212", 
                 "112232", "122132", "122231", "113222", "123122", "123221", 
                 "223211", "221132", "221231", "213212", "223112", "312131", 
                 "311222", "321122", "321221", "312212", "322112", "322211", 
                 "212123", "212321", "232121", "111323", "131123", "131321", 
                 "112313", "132113", "132311", "211313", "231113", "231311", 
                 "112133", "112331", "132131", "113123", "113321", "133121", 
                 "313121", "211331", "231131", "213113", "213311", "213131", 
                 "311123", "311321", "331121", "312113", "312311", "332111", 
                 "314111", "221411", "431111", "111224", "111422", "121124", 
                 "121421", "141122", "141221", "112214", "112412", "122114", 
                 "122411", "142112", "142211", "241211", "221114", "413111", 
                 "241112", "134111", "111242", "121142", "121241", "114212", 
                 "124112", "124211", "411212", "421112", "421211", "212141", 
                 "214121", "412121", "111143", "111341", "131141", "114113", 
                 "114311", "411113", "411311", "113141", "114131", "311141", 
                 "411131", "211412", "211214", "211232", "233111"};

            if ((pattern != null) && (pattern.Length >= 6))
            {
                for (int i = 0; i < Code128.Length; i++)
                {
                    if (pattern.StartsWith(Code128[i]))
                    {
                        if (i == CODE128STOP)
                        {
                            try
                            {
                                int length = result.Length;
                                if (length > 1)
                                {
                                    char checkDigit;
                                    if (codePage == 'C')
                                    {
                                        checkDigit = (char)(Convert.ToInt32(result.Substring(length - 2)) + 32);
                                        result = result.Substring(0, length - 2);
                                    }
                                    else
                                    {
                                        checkDigit = result[length - 1];
                                        result = result.Substring(0, length - 1);
                                    }
                                    uint uCheckDigit = (uint)((checkDigit - 32) * codes);
                                    if (checkSum > uCheckDigit)
                                    {
                                        checkSum = (checkSum - uCheckDigit) % 103;
                                        if (checkDigit == (char)((int)(checkSum + 32)))
                                        {
                                            return CODE128STOP;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //System.Diagnostics.Trace.Write(ex);
                            }
                            // If reach this point, some check failed.
                            // Reset everything and return error.
                            result = string.Empty;
                            checkSum = 0;
                            codes = 0;
                            return -2;
                        }
                        else if (i >= CODE128START)
                        {
                            // Start new code 128 sequence
                            result = string.Empty;
                            checkSum = (uint)i;
                            codePage = (char)('A' + (i - CODE128START));
                        }
                        else if (checkSum > 0)
                        {
                            bool shouldSkip = false;
                            char newCodePage = codePage;
                            switch (i)
                            {
                                case CODE128C:
                                    newCodePage = 'C';
                                    break;
                                case CODE128B:
                                    newCodePage = 'B';
                                    break;
                                case CODE128A:
                                    newCodePage = 'A';
                                    break;
                            }
                            if (codePage != newCodePage)
                            {
                                codePage = newCodePage;
                                shouldSkip = true;
                            }
                            if (!shouldSkip)
                            {
                                switch (codePage)
                                {
                                    case 'C':
                                        result += i.ToString("00");
                                        break;
                                    default:
                                        // Regular character
                                        char c = (char)(i + 32);
                                        result += c;
                                        break;
                                }
                            }
                            codes++;
                            checkSum += (uint)(i * codes);
                        }
                        return i;
                    }
                }
            }
            result = string.Empty;
            checkSum = 0;
            codes = 0;
            return -1;
        }
        #endregion
    }
}