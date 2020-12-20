using System;
using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop
{
    internal static class Kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
