using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop;

internal static class InteropUtilities
{
    /// <exception cref="COMException"></exception>
    public static BOOL EnsureNonZero(this BOOL value)
    {
        if (value.Value == 0)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        return value;
    }

    /// <exception cref="COMException"></exception>
    public static BOOL EnsureNonMinusOne(this BOOL value)
    {
        if (value.Value == -1)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        return value;
    }
}
