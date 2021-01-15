using System;
using System.Linq;
using H.Hooks.Core.Interop.WinUser;

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyboardHookEventArgs
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAltPressed => IsLAltPressed || IsRAltPressed;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLAltPressed { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRAltPressed { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCtrlPressed => IsLCtrlPressed || IsRCtrlPressed;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLCtrlPressed { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRCtrlPressed { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsShiftPressed => IsLShiftPressed || IsRShiftPressed;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLShiftPressed { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRShiftPressed { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsWinPressed => IsLWinPressed || IsRWinPressed;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLWinPressed { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRWinPressed { get; }

        #endregion

        #region Constructors

        internal KeyboardHookEventArgs(KeyboardHookStruct lParam)
        {
            Key = (Key)lParam.VirtualKeyCode;

            //Control.ModifierKeys doesn't capture alt/win, and doesn't have r/l granularity
            IsLAltPressed = Check(Key, VirtualKey.LeftAlt, Key.LMenu);
            IsRAltPressed = Check(Key, VirtualKey.RightAlt, Key.RMenu);

            IsLCtrlPressed = Check(Key, VirtualKey.LeftControl, Key.LControlKey);
            IsRCtrlPressed = Check(Key, VirtualKey.RightControl, Key.RControlKey);

            IsLShiftPressed = Check(Key, VirtualKey.LeftShift, Key.LShiftKey);
            IsRShiftPressed = Check(Key, VirtualKey.RightShift, Key.RShiftKey);

            IsLWinPressed = Check(Key, VirtualKey.LeftWin, Key.LWin);
            IsRWinPressed = Check(Key, VirtualKey.RightWin, Key.RWin);

            if (new[] { Key.LMenu, Key.RMenu, Key.LControlKey, Key.RControlKey, Key.LShiftKey, Key.RShiftKey, Key.LWin, Key.RWin }
                .Contains(Key))
            {
                Key = Key.None;
            }
        }

        #endregion

        #region Methods

        private static bool Check(Key key, VirtualKey virtualKey, Key realKey)
        {
            return key == realKey ||
                   Convert.ToBoolean(User32.GetKeyState((int)virtualKey) & (int) KeyFlag.Up);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Key={Key}; Win={IsWinPressed}; Alt={IsAltPressed}; Ctrl={IsCtrlPressed}; Shift={IsShiftPressed}";
        }

        #endregion
    }
}