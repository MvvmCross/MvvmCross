using System.Diagnostics;

namespace System
{
    public static class Console
    {
        public static ConsoleColor ForegroundColor { get; set; }

        public static void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }

    }

}
