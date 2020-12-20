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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static T ToStructure<T>(IntPtr ptr) where T : struct
        {
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }
    }
}
