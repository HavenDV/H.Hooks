using System;
using System.Linq;
using H.Hooks.Core.Interop.WinUser;

namespace H.Hooks
{
    public class KeyboardHookEventArgs
    {
        #region Properties

        public Key Key { get; }
        public bool Handled { get; set; }

        public bool IsAltPressed => IsLAltPressed || IsRAltPressed;
        public bool IsLAltPressed { get; }
        public bool IsRAltPressed { get; }

        public bool IsCtrlPressed => IsLCtrlPressed || IsRCtrlPressed;
        public bool IsLCtrlPressed { get; }
        public bool IsRCtrlPressed { get; }

        public bool IsShiftPressed => IsLShiftPressed || IsRShiftPressed;
        public bool IsLShiftPressed { get; }
        public bool IsRShiftPressed { get; }

        public bool IsWinPressed => IsLWinPressed || IsRWinPressed;
        public bool IsLWinPressed { get; }
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
                   Convert.ToBoolean(User32.GetKeyState(virtualKey) & (int) KeyFlag.Up);
        }

        public override string ToString()
        {
            return $"Key={Key}; Win={IsWinPressed}; Alt={IsAltPressed}; Ctrl={IsCtrlPressed}; Shift={IsShiftPressed}";
        }

        #endregion
    }
}