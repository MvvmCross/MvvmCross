using System;
using System.Text;

namespace Phone7.Fx.Imaging
{
    public class EANParser
    {
        #region EAN-specific
        /// <summary>
        /// Parses EAN-barcodes from the input pattern.
        /// </summary>
        /// <param name="sbPattern">Input pattern, should contain characters
        /// "1" .. "4" to indicate valid EAN bar widths.</param>
        /// <returns>Pipe-separated list of barcodes, empty string if none were detected</returns>
        internal static string ParseCode(StringBuilder pattern)
        {
            StringBuilder EANData = new StringBuilder(32);
            int eANSeparators = 0;
            string eANCode = string.Empty;

            int pos = 0;
            pattern.Append("|"); // append one extra "gap" character because separator has only 3 bands
            while (pos <= pattern.Length - 4)
            {
                string eANDigit = ParseEANPattern(pattern.ToString(pos, 4), eANCode, eANSeparators);
                switch (eANDigit)
                {
                    case null:
                        // reset on invalid code
                        //iEANSeparators = 0;
                        eANCode = string.Empty;
                        pos++;
                        break;
                    case "|":
                        // EAN separator found. Each EAN code contains three separators.
                        if (eANSeparators >= 3)
                            eANSeparators = 1;
                        else
                            eANSeparators++;
                        pos += 3;
                        if (eANSeparators == 2)
                        {
                            pos += 2; // middle separator has 5 bars
                        }
                        else if (eANSeparators == 3) // end of EAN code detected
                        {
                            string firstDigit = GetEANFirstDigit(ref eANCode);
                            if ((firstDigit != null) && (eANCode.Length == 12))
                            {
                                eANCode = firstDigit + eANCode;
                                if (EANData.Length > 0)
                                    EANData.Append("|");
                                EANData.Append(eANCode);
                            }
                            // reset after end of code
                            //iEANSeparators = 0;
                            eANCode = string.Empty;
                        }
                        break;
                    case "S":
                        // Start of supplemental code after EAN code
                        pos += 3;
                        eANCode = "S";
                        eANSeparators = 1;
                        break;
                    default:
                        if (eANSeparators > 0)
                        {
                            eANCode += eANDigit;
                            pos += 4;
                            if (eANCode.StartsWith("S"))
                            {
                                // Each digit of the supplemental code is followed by an additional "11"
                                // We assume that the code ends if that is no longer the case.
                                if ((pattern.Length > pos + 2) && (pattern.ToString(pos, 2) == "11"))
                                    pos += 2;
                                else
                                {
                                    // Supplemental code ends. It must be either 2 or 5 digits.
                                    eANCode = CheckEANSupplement(eANCode);
                                    if (eANCode.Length > 0)
                                    {
                                        if (EANData.Length > 0)
                                            EANData.Append("|");
                                        EANData.Append(eANCode);
                                    }
                                    // reset after end of code
                                    eANSeparators = 0;
                                    eANCode = string.Empty;
                                }
                            }
                        }
                        else
                            pos++; // no EAN digit expected before first separator
                        break;
                }
            }
            return EANData.ToString();
        }

       

        /// <summary>
        /// Parses the EAN pattern for one digit or separator
        /// </summary>
        /// <param name="pattern">Pattern to be parsed</param>
        /// <param name="eANCode">EAN code found so far</param>
        /// <param name="eANSeparators">Number of separators found so far</param>
        /// <returns>Detected digit type (L/G/R) and digit, "|" for separator
        /// or null if the pattern was not recognized.</returns>
        private static string ParseEANPattern(string pattern, string eANCode, int eANSeparators)
        {
            string[] LRCodes = 
                {"3211", "2221", "2122", "1411", "1132", 
                 "1231", "1114", "1312", "1213", "3112"};
            string[] GCodes = 
                {"1123", "1222", "2212", "1141", "2311",
                 "1321", "4111", "2131", "3121", "2113"};
            if ((pattern != null) && (pattern.Length >= 3))
            {
                if (pattern.StartsWith("111") && ((eANSeparators * 12) == eANCode.Length))
                    return "|";   // found separator
                if (pattern.StartsWith("112") && (eANSeparators == 3) && (eANCode.Length == 0))
                    return "S";   // found EAN supplemental code

                for (int i = 0; i < 10; i++)
                {
                    if (pattern.StartsWith(LRCodes[i]))
                        return ((eANSeparators == 2) ? "R" : "L") + i;
                    if (pattern.StartsWith(GCodes[i]))
                        return "G" + i;
                }
            }
            return null;
        }

        /// <summary>
        /// Decodes the L/G-pattern for the left half of the EAN code 
        /// to derive the first digit. See table in
        /// http://en.wikipedia.org/wiki/European_Article_Number
        /// </summary>
        /// <param name="eANPattern">
        /// IN: EAN pattern with digits and L/G/R codes.
        /// OUT: EAN digits only.
        /// </param>
        /// <returns>Detected first digit or null.</returns>
        private static string GetEANFirstDigit(ref string eANPattern)
        {
            string[] LGPatterns = 
        {"LLLLLL", "LLGLGG", "LLGGLG", "LLGGGL", "LGLLGG",
         "LGGLLG", "LGGGLL", "LGLGLG", "LGLGGL", "LGGLGL"};
            string sLG = string.Empty;
            string sDigits = string.Empty;
            if ((eANPattern != null) && (eANPattern.Length >= 24))
            {
                for (int i = 0; i < 12; i++)
                {
                    sLG += eANPattern[2 * i];
                    sDigits += eANPattern[2 * i + 1];
                }
                for (int i = 0; i < 10; i++)
                {
                    if (sLG.StartsWith(LGPatterns[i]))
                    {
                        eANPattern = sDigits + eANPattern.Substring(24);
                        return i.ToString();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if EAN supplemental code is valid.
        /// </summary>
        /// <param name="eANPattern">Parse result</param>
        /// <returns>Supplemental code or empty string</returns>
        private static string CheckEANSupplement(string eANPattern)
        {
            try
            {
                if (eANPattern.StartsWith("S"))
                {
                    string digits = string.Empty;
                    string lG = string.Empty;
                    for (int i = 1; i < eANPattern.Length - 1; i += 2)
                    {
                        lG += eANPattern[i];
                        digits += eANPattern[i + 1];
                    }

                    // Supplemental code must be either 2 or 5 digits.
                    switch (digits.Length)
                    {
                        case 2:
                            // Do EAN-2 parity check
                            string[] EAN2Parity = { "LL", "LG", "GL", "GG" };
                            int iParity = Convert.ToInt32(digits) % 4;
                            if (lG != EAN2Parity[iParity])
                                return string.Empty; // parity check failed
                            break;
                        case 5:
                            // Do EAN-5 checksum validation
                            uint uCheckSum = 0;
                            for (int i = 0; i < digits.Length; i++)
                            {
                                uCheckSum += (uint)(Convert.ToUInt32(digits.Substring(i, 1)) * (((i & 1) == 0) ? 3 : 9));
                            }
                            string[] EAN5CheckSumPattern = 
                {"GGLLL", "GLGLL", "GLLGL", "GLLLG", "LGGLL", 
                 "LLGGL", "LLLGG", "LGLGL", "LGLLG", "LLGLG"};
                            if (lG != EAN5CheckSumPattern[uCheckSum % 10])
                                return string.Empty; // Checksum validation failed
                            break;
                        default:
                            return string.Empty;
                    }
                    return "S" + digits;
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Trace.Write(ex);
            }
            return string.Empty;
        }
        #endregion
    }
}