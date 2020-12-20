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
        public static nint Check(this nint ptr)
        {
            if (ptr == 0)
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
        public static T ToStructure<T>(nint ptr) where T : struct
        {
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }
    }
}
