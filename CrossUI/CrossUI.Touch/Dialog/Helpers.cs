using System;

namespace CrossUI.Touch.Dialog
{
    internal static class Helpers
    {
        public static nint Min(nint val1, nint val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        public static nfloat Min(nfloat val1, nfloat val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        public static nint Max(nint val1, nint val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        public static nfloat Max(nfloat val1, nfloat val2)
        {
            return val1 > val2 ? val1 : val2;
        }
    }
}