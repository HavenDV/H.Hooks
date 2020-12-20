using System;

namespace H.Hooks.Core.Interop.WinUser
{
    /// <summary>
    /// An application-defined or library-defined callback function used with the SetWindowsHookEx function. The system calls this function whenever an application calls the GetMessage or PeekMessage function and there is a keyboard message (WM_KEYUP or WM_KEYDOWN) to be processed.
    /// </summary>
    /// <param name="code">A code the hook procedure uses to determine how to process the message. If code is less than zero, the hook procedure must pass the message to the CallNextHookEx function without further processing and should return the value returned by CallNextHookEx.</param>
    /// <param name="wParam">The virtual-key code of the key that generated the keystroke message.</param>
    /// <param name="lParam">The repeat count, scan code, extended-key flag, context code, previous key-state flag, and transition-state flag. For more information about the lParam parameter, see Keystroke Message Flags.</param>
    /// <returns>If code is less than zero, the hook procedure must return the value returned by CallNextHookEx. If code is greater than or equal to zero, and the hook procedure did not process the message, it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise bad stuff.</returns>
    internal delegate IntPtr HookProc(int code, int wParam, IntPtr lParam);
}