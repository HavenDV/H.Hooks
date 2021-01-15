#pragma warning disable CA1707

// ReSharper disable InconsistentNaming
namespace H.Hooks.Core.Interop.WinUser
{
    /// <summary>
    /// WH_ prefix SetWindowsHook() codes. <br/>
    /// Values from Winuser.h in Microsoft SDK. <br/>
    /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.10240.0/um/WinUser.h#L765
    /// </summary>
    internal static class WH
    {
        /// <summary>
        /// Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure. 
        /// </summary>
        public const int KEYBOARD = 2;

        /// <summary>
        /// Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure. 
        /// </summary>
        public const int MOUSE = 7;

        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level keyboard input events.
        /// </summary>
        public const int KEYBOARD_LL = 13;

        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
        /// </summary>
        public const int MOUSE_LL = 14;
    }
}