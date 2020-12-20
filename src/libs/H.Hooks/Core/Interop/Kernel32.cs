using System;
using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop
{
    internal static class Kernel32
    {
        /// <summary>
        /// If this parameter is NULL, GetModuleHandle returns a handle to the file used to create the calling process (.exe file).
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint GetModuleHandle(string? lpModuleName);
    }
}
