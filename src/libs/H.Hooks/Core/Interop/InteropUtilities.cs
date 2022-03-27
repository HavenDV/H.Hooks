using System.ComponentModel;
using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop;

internal static class InteropUtilities
{
    /// <summary>
    /// Throws <see cref="Win32Exception"/> with <see cref="Marshal.GetLastWin32Error"/>
    /// if ptr is 0.
    /// </summary>
    /// <param name="ptr"></param>
    /// <exception cref="Win32Exception"></exception>
    /// <returns>Returns input ptr.</returns>
    public static nint Check(this nint ptr)
    {
        if (ptr == 0)
        {
            ThrowWin32Exception();
        }

        return ptr;
    }

    /// <summary>
    /// Throws <see cref="Win32Exception"/> with <see cref="Marshal.GetLastWin32Error"/>
    /// if value is <see langword="false"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="Win32Exception"></exception>
    /// <returns></returns>
    public static void Check(this bool value)
    {
        if (!value)
        {
            ThrowWin32Exception();
        }
    }

    /// <summary>
    /// Throws <see cref="Win32Exception"/> with <see cref="Marshal.GetLastWin32Error"/>
    /// if value is <see langword="false"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="Win32Exception"></exception>
    /// <returns></returns>
    public static void Check(this BOOL value)
    {
        if (!value)
        {
            ThrowWin32Exception();
        }
    }

    /// <summary>
    /// Throws <see cref="Win32Exception"/> with <see cref="Marshal.GetLastWin32Error"/>.
    /// </summary>
    /// <exception cref="Win32Exception"></exception>
    /// <returns>It always throws exception.</returns>
    public static void ThrowWin32Exception()
    {
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ptr"></param>
    /// <returns></returns>
    public static T ToStructure<T>(nint ptr) where T : struct
    {
        return
            (T?)Marshal.PtrToStructure(ptr, typeof(T)) ?? 
            throw new InvalidOperationException($"{nameof(Marshal.PtrToStructure)} returns null.");
    }
}
