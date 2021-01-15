namespace H.Hooks.Core.Interop.WinUser
{
    /// <summary>
    /// WH_ prefix SetWindowsHook() codes. <br/>
    /// Values from Winuser.h in Microsoft SDK. <br/>
    /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.10240.0/um/WinUser.h#L765
    /// </summary>
    public enum HookProcedureType
    {
        /// <summary>
        /// Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure. 
        /// </summary>
        Keyboard = 2,

        /// <summary>
        /// Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure. 
        /// </summary>
        Mouse = 7,

        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level keyboard input events.
        /// </summary>
        KeyboardLowLevel = 13,

        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
        /// </summary>
        MouseLowLevel = 14,
    }
}