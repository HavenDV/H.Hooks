using System;
using System.ComponentModel;
using H.Hooks.Core.Interop;

namespace H.Hooks.Core
{
    public static class Kernel32Methods
    {
        /// <summary>
        /// Do not pass a handle returned by GetModuleHandle to the FreeLibrary function. <br/>
        /// Doing so can cause a DLL module to be unmapped prematurely.
        /// </summary>
        /// <exception cref="Win32Exception"></exception>
        /// <returns></returns>
        public static IntPtr GetCurrentProcessModuleHandle()
        {
            return Kernel32.GetModuleHandle(null).Check();
        }
    }
}
