using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop
{
    internal static class InteropUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <exception cref="Win32Exception"></exception>
        /// <returns></returns>
        public static IntPtr Check(this IntPtr ptr)
        {
            if (ptr == null || ptr == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return ptr;
        } 
    }
}
