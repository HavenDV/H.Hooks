using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop.WinUser
{
    internal static class User32
    {
        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events.
        /// These events are associated either with a specific thread or
        /// with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">The type of hook procedure to be installed.</param>
        /// <param name="lpfn">
        /// A pointer to the hook procedure. If the dwThreadId parameter is zero or
        /// specifies the identifier of a thread created by a different process,
        /// the lpfn parameter must point to a hook procedure in a DLL.
        /// Otherwise, lpfn can point to a hook procedure in the code
        /// associated with the current process.
        /// </param>
        /// <param name="hMod">
        /// A handle to the DLL containing the hook procedure pointed to by the lpfn parameter.
        /// The hMod parameter must be set to NULL if the dwThreadId parameter specifies
        /// a thread created by the current process and if the hook procedure is within
        /// the code associated with the current process.
        /// </param>
        /// <param name="dwThreadId">
        /// The identifier of the thread with which the hook procedure is to be associated.
        /// If this parameter is zero, the hook procedure is associated with all existing threads
        /// running in the same desktop as the calling thread.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the hook procedure. <br/>
        /// If the function fails, the return value is NULL.
        /// To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern nint SetWindowsHookEx(
            HookProcedureType idHook, 
            HookProc lpfn, 
            nint hMod, 
            uint dwThreadId);

        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hhk">
        /// A handle to the hook to be removed.
        /// This parameter is a hook handle obtained by a previous call to SetWindowsHookEx.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. <br/>
        /// If the function fails, the return value is zero.
        /// To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// The hook procedure can be in the state of being called by another thread
        /// even after UnhookWindowsHookEx returns. If the hook procedure is not being called concurrently,
        /// the hook procedure is removed immediately before UnhookWindowsHookEx returns.
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(
            nint hhk);

        /// <summary>
        /// Passes the hook information to the next hook procedure in the current hook chain.
        /// A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hhk">This parameter is ignored.</param>
        /// <param name="nCode">
        /// The hook code passed to the current hook procedure.
        /// The next hook procedure uses this code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        /// The wParam value passed to the current hook procedure.
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <param name="lParam">
        /// The lParam value passed to the current hook procedure.
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <returns>
        /// This value is returned by the next hook procedure in the chain.
        /// The current hook procedure must also return this value.
        /// The meaning of the return value depends on the hook type.
        /// For more information, see the descriptions of the individual hook procedures.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern nint CallNextHookEx(
            nint hhk, 
            int nCode, 
            int wParam, 
            nint lParam);

        /// <summary>
        /// Retrieves the status of the specified virtual key.
        /// The status specifies whether the key is up, down,
        /// or toggled (on, off—alternating each time the key is pressed).
        /// </summary>
        /// <param name="key">
        /// A virtual key. If the desired virtual key is a letter or
        /// digit (A through Z, a through z, or 0 through 9),
        /// nVirtKey must be set to the ASCII value of that character.
        /// For other keys, it must be a virtual-key code. <br/>
        /// If a non-English keyboard layout is used,
        /// virtual keys with values in the range ASCII A through Z and 0 through 9 are used
        /// to specify most of the character keys. For example, for the German keyboard layout,
        /// the virtual key of value ASCII O (0x4F) refers to the "o" key,
        /// whereas VK_OEM_1 refers to the "o with umlaut" key.
        /// </param>
        /// <returns>
        /// The return value specifies the status of the specified virtual key, as follows: <br/>
        /// If the high-order bit is 1, the key is down; otherwise, it is up. <br/>
        /// If the low-order bit is 1, the key is toggled.
        /// A key, such as the CAPS LOCK key, is toggled if it is turned on.
        /// The key is off and untoggled if the low-order bit is 0.
        /// A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled,
        /// and off when the key is untoggled.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern short GetKeyState(
            int key);

        /// <summary>
        /// Copies the status of the 256 virtual keys to the specified buffer.
        /// </summary>
        /// <param name="lpKeyState">The 256-byte array that receives the status data for each virtual key.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. <br/>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// In some cases this function will always return the same array,
        /// independent of actual keyboard state.
        /// This is due to Windows not updating the virtual key array internally.
        /// It has been found that declaring and calling GetKeyState on any key
        /// before calling GetKeyboardState will solve this issue.
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// Dispatches incoming sent messages,
        /// checks the thread message queue for a posted message,
        /// and retrieves the message (if any exist).
        /// </summary>
        /// <param name="lpMsg">A pointer to an MSG structure that receives message information.</param>
        /// <param name="hWnd">
        /// A handle to the window whose messages are to be retrieved.
        /// The window must belong to the current thread. <br/>
        /// If hWnd is NULL, PeekMessage retrieves messages for any window
        /// that belongs to the current thread, and any messages
        /// on the current thread's message queue whose hwnd value is NULL (see the MSG structure).
        /// Therefore if hWnd is NULL, both window messages and thread messages are processed. <br/>
        /// If hWnd is -1, PeekMessage retrieves only messages
        /// on the current thread's message queue whose hwnd value is NULL,
        /// that is, thread messages as posted
        /// by PostMessage (when the hWnd parameter is NULL) or PostThreadMessage.
        /// </param>
        /// <param name="wMsgFilterMin">
        /// The integer value of the lowest message value to be retrieved.
        /// Use WM_KEYFIRST (0x0100) to specify the first keyboard message or
        /// WM_MOUSEFIRST (0x0200) to specify the first mouse message. <br/>
        /// If wMsgFilterMin and wMsgFilterMax are both zero,
        /// GetMessage returns all available messages (that is, no range filtering is performed).
        /// </param>
        /// <param name="wMsgFilterMax">
        /// The integer value of the highest message value to be retrieved.
        /// Use WM_KEYLAST to specify the last keyboard message or
        /// WM_MOUSELAST to specify the last mouse message. <br/>
        /// If wMsgFilterMin and wMsgFilterMax are both zero,
        /// GetMessage returns all available messages (that is, no range filtering is performed).
        /// </param>
        /// <param name="wRemoveMsg">
        /// Specifies how messages are to be handled.
        /// This parameter can be one or more of the following values: <see cref="PM"/>. <br/>
        /// By default, all message types are processed.
        /// To specify that only certain message should be processed,
        /// specify one or more of the following values: <see cref="PM"/>.
        /// </param>
        /// <returns>
        /// If a message is available, the return value is nonzero. <br/>
        /// If no messages are available, the return value is zero.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern bool PeekMessage(
            out NativeMessage lpMsg, 
            nint hWnd, 
            uint wMsgFilterMin,
            uint wMsgFilterMax, 
            uint wRemoveMsg);

        /// <summary>
        /// Retrieves a message from the calling thread's message queue.
        /// The function dispatches incoming sent messages until a posted message is available for retrieval. <br/>
        /// Unlike GetMessage, the PeekMessage function does not wait for a message to be posted before returning.
        /// </summary>
        /// <param name="lpMsg">
        /// A pointer to an MSG structure that receives message information from the thread's message queue.
        /// </param>
        /// <param name="hWnd">
        /// A handle to the window whose messages are to be retrieved.
        /// The window must belong to the current thread. <br/>
        /// If hWnd is NULL, GetMessage retrieves messages for any window that belongs to the current thread,
        /// and any messages on the current thread's message queue whose hwnd value is NULL (see the MSG structure).
        /// Therefore if hWnd is NULL, both window messages and thread messages are processed. <br/>
        /// If hWnd is -1, GetMessage retrieves only messages on the current thread's message queue
        /// whose hwnd value is NULL, that is, thread messages as posted
        /// by PostMessage (when the hWnd parameter is NULL) or PostThreadMessage.
        /// </param>
        /// <param name="wMsgFilterMin">
        /// The integer value of the lowest message value to be retrieved.
        /// Use WM_KEYFIRST (0x0100) to specify the first keyboard message or
        /// WM_MOUSEFIRST (0x0200) to specify the first mouse message. <br/>
        /// Use WM_INPUT here and in wMsgFilterMax to specify only the WM_INPUT messages. <br/>
        /// If wMsgFilterMin and wMsgFilterMax are both zero,
        /// GetMessage returns all available messages (that is, no range filtering is performed).
        /// </param>
        /// <param name="wMsgFilterMax">
        /// The integer value of the highest message value to be retrieved.
        /// Use WM_KEYLAST to specify the last keyboard message or
        /// WM_MOUSELAST to specify the last mouse message. <br/>
        /// Use WM_INPUT here and in wMsgFilterMin to specify only the WM_INPUT messages. <br/>
        /// If wMsgFilterMin and wMsgFilterMax are both zero,
        /// GetMessage returns all available messages (that is, no range filtering is performed).
        /// </param>
        /// <returns>
        /// If the function retrieves a message other than WM_QUIT, the return value is nonzero. <br/>
        /// If the function retrieves the WM_QUIT message, the return value is zero. <br/>
        /// If there is an error, the return value is -1. For example, the function fails
        /// if hWnd is an invalid window handle or lpMsg is an invalid pointer.
        /// To get extended error information, call GetLastError. <br/>
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int GetMessage(
            out NativeMessage lpMsg, 
            nint hWnd, 
            uint wMsgFilterMin,
            uint wMsgFilterMax);

        /// <summary>
        /// Calls the default window procedure to provide default processing
        /// for any window messages that an application does not process.
        /// This function ensures that every message is processed.
        /// DefWindowProc is called with the same parameters received by the window procedure.
        /// </summary>
        /// <param name="hWnd">A handle to the window procedure that received the message.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">
        /// Additional message information.
        /// The content of this parameter depends on the value of the Msg parameter.
        /// </param>
        /// <param name="lParam">
        /// Additional message information.
        /// The content of this parameter depends on the value of the Msg parameter.
        /// </param>
        /// <returns>
        /// The return value is the result of the message processing and depends on the message.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern nint DefWindowProc(
            nint hWnd, 
            uint msg, 
            nint wParam, 
            nint lParam);

        /// <summary>
        /// Posts a message to the message queue of the specified thread.
        /// It returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="idThread">
        /// The identifier of the thread to which the message is to be posted. <br/>
        /// The function fails if the specified thread does not have a message queue.
        /// The system creates a thread's message queue when the thread makes its first call
        /// to one of the User or GDI functions. For more information, see the Remarks section. <br/>
        /// Message posting is subject to UIPI. The thread of a process can post messages only
        /// to posted-message queues of threads in processes of lesser or equal integrity level. <br/>
        /// This thread must have the SE_TCB_NAME privilege to post a message to a thread that belongs
        /// to a process with the same locally unique identifier (LUID) but is in a different desktop.
        /// Otherwise, the function fails and returns ERROR_INVALID_THREAD_ID. <br/>
        /// This thread must either belong to the same desktop as the calling thread or to a process
        /// with the same LUID. Otherwise, the function fails and returns ERROR_INVALID_THREAD_ID.
        /// </param>
        /// <param name="msg">The type of message to be posted.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. <br/>
        /// If the function fails, the return value is zero.
        /// To get extended error information, call GetLastError.
        /// GetLastError returns ERROR_INVALID_THREAD_ID if idThread is not a valid thread identifier,
        /// or if the thread specified by idThread does not have a message queue.
        /// GetLastError returns ERROR_NOT_ENOUGH_QUOTA when the message limit is hit.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern bool PostThreadMessage(
            uint idThread, 
            uint msg, 
            nint wParam, 
            nint lParam);
    }
}
