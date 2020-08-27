using System;
using System.Runtime.InteropServices;

namespace RedrawPerformance
{
    public static class Interop
    {
        [Flags]
        public enum RDW : uint
        {
            INVALIDATE = 0x0001,
            INTERNALPAINT = 0x0002,
            ERASE = 0x0004,
            VALIDATE = 0x0008,
            NOINTERNALPAINT = 0x0010,
            NOERASE = 0x0020,
            NOCHILDREN = 0x0040,
            ALLCHILDREN = 0x0080,
            UPDATENOW = 0x0100,
            ERASENOW = 0x0200,
            FRAME = 0x0400,
            NOFRAME = 0x0800,
        }

        [DllImport("User32.dll", ExactSpelling = true)]
        public static extern int RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RDW flags);

        [DllImport("User32.dll", ExactSpelling = true)]
        public static extern int UpdateWindow(IntPtr hWnd);
    }
}
