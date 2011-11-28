using System.Text;

namespace Phone7.Fx.Imaging
{
    internal class Code39Parser
    {
        // Code39
        private const string STARPATTERN = "nwnnwnwnn"; // the pattern representing a star in Code39
        
        /// <summary>
        /// Parses Code39 barcodes from the input pattern.
        /// </summary>
        /// <param name="sbPattern">Input pattern, should contain "n"-characters to
        /// indicate narrow bars and "w" to indicate wide bars.</param>
        /// <param name="pattern"></param>
        /// <returns>Pipe-separated list of barcodes, empty string if none were detected</returns>
        internal static string ParseCode(StringBuilder pattern)
        {
            // Each pattern within code 39 is nine bars with one white bar between each pattern
            if (pattern.Length > 9)
            {
                StringBuilder barcodes = new StringBuilder();
                int starPattern = pattern.ToString().IndexOf(STARPATTERN); // index of first star barcode in pattern
                while ((starPattern >= 0) && (starPattern <= pattern.Length - 9))
                {
                    int pos = starPattern;
                    int noise = 0;
                    StringBuilder data = new StringBuilder(pattern.Length / 10);
                    while (pos <= pattern.Length - 9)
                    {
                        
                        // Test the next 9 characters from the pattern string
                        string dataParsed = ParseCode39Pattern(pattern.ToString(pos, 9));

                        if (dataParsed == null) // no recognizeable data
                        {
                            pos++;
                            noise++;
                        }
                        else
                        {
                            // record if the data contained a lot of noise before the next valid data
                            if (noise >= 2)
                                data.Append("|");
                            noise = 0; // reset noise counter
                            data.Append(dataParsed);
                            pos += 10;
                        }
                    }
                    if (data.Length > 0)
                    {
                        // look for valid Code39 patterns in the data.
                        // A valid Code39 pattern starts and ends with "*" and does not contain a noise character "|".
                        // We return a pipe-separated list of these patterns.
                        string[] asBarcodes = data.ToString().Split('|');
                        foreach (string sBarcode in asBarcodes)
                        {
                            if (sBarcode.Length > 2)
                            {
                                int iFirstStar = sBarcode.IndexOf("*");
                                if ((iFirstStar >= 0) && (iFirstStar < sBarcode.Length - 1))
                                {
                                    int iSecondStar = sBarcode.IndexOf("*", iFirstStar + 1);
                                    if (iSecondStar > iFirstStar + 1)
                                    {
                                        barcodes.Append(sBarcode.Substring(iFirstStar + 1, (iSecondStar - iFirstStar - 1)) + "|");
                                    }
                                }
                            }
                        }
                    }
                    starPattern = pattern.ToString().IndexOf(STARPATTERN, starPattern + 5); // "nwnnwnwnn" pattern can not occur again before current index + 5
                }
                if (barcodes.Length > 1)
                    return barcodes.ToString(0, barcodes.Length - 1);
            }
            return string.Empty;
        }

        /// <summary>
        /// Parses bar pattern for one Code39 character.
        /// </summary>
        /// <param name="pattern">Pattern to be examined, should be 9 characters</param>
        /// <returns>Detected character or null</returns>
        private static string ParseCode39Pattern(string pattern)
        {
            switch (pattern)
            {
                case "wnnwnnnnw":
                    return "1";
                case "nnwwnnnnw":
                    return "2";
                case "wnwwnnnnn":
                    return "3";
                case "nnnwwnnnw":
                    return "4";
                case "wnnwwnnnn":
                    return "5";
                case "nnwwwnnnn":
                    return "6";
                case "nnnwnnwnw":
                    return "7";
                case "wnnwnnwnn":
                    return "8";
                case "nnwwnnwnn":
                    return "9";
                case "nnnwwnwnn":
                    return "0";
                case "wnnnnwnnw":
                    return "A";
                case "nnwnnwnnw":
                    return "B";
                case "wnwnnwnnn":
                    return "C";
                case "nnnnwwnnw":
                    return "D";
                case "wnnnwwnnn":
                    return "E";
                case "nnwnwwnnn":
                    return "F";
                case "nnnnnwwnw":
                    return "G";
                case "wnnnnwwnn":
                    return "H";
                case "nnwnnwwnn":
                    return "I";
                case "nnnnwwwnn":
                    return "J";
                case "wnnnnnnww":
                    return "K";
                case "nnwnnnnww":
                    return "L";
                case "wnwnnnnwn":
                    return "M";
                case "nnnnwnnww":
                    return "N";
                case "wnnnwnnwn":
                    return "O";
                case "nnwnwnnwn":
                    return "P";
                case "nnnnnnwww":
                    return "Q";
                case "wnnnnnwwn":
                    return "R";
                case "nnwnnnwwn":
                    return "S";
                case "nnnnwnwwn":
                    return "T";
                case "wwnnnnnnw":
                    return "U";
                case "nwwnnnnnw":
                    return "V";
                case "wwwnnnnnn":
                    return "W";
                case "nwnnwnnnw":
                    return "X";
                case "wwnnwnnnn":
                    return "Y";
                case "nwwnwnnnn":
                    return "Z";
                case "nwnnnnwnw":
                    return "-";
                case "wwnnnnwnn":
                    return ".";
                case "nwwnnnwnn":
                    return " ";
                case STARPATTERN:
                    return "*";
                case "nwnwnwnnn":
                    return "$";
                case "nwnwnnnwn":
                    return "/";
                case "nwnnnwnwn":
                    return "+";
                case "nnnwnwnwn":
                    return "%";
                default:
                    return null;
            }
        }
    }
}