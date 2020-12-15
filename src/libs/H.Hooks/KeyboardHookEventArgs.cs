using System;
using System.Linq;

namespace H.NET.Utilities
{
    public class KeyboardHookEventArgs
    {
        #region PInvoke

        private const int KeyPressed = 0x8000;

        #endregion

        public Keys Key { get; }
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

        private static bool Check(Keys key, Win32.VirtualKey virtualKey, Keys realKey) =>
            Convert.ToBoolean(Win32.GetKeyState(virtualKey) & KeyPressed) || key == realKey;

        internal KeyboardHookEventArgs(Win32.KeyboardHookStruct lParam)
        {
            Key = (Keys)lParam.VirtualKeyCode;

            //Control.ModifierKeys doesn't capture alt/win, and doesn't have r/l granularity
            IsLAltPressed = Check(Key, Win32.VirtualKey.LAlt, Keys.LMenu);
            IsRAltPressed = Check(Key, Win32.VirtualKey.RAlt, Keys.RMenu);

            IsLCtrlPressed = Check(Key, Win32.VirtualKey.LControl, Keys.LControlKey);
            IsRCtrlPressed = Check(Key, Win32.VirtualKey.RControl, Keys.RControlKey);

            IsLShiftPressed = Check(Key, Win32.VirtualKey.LShift, Keys.LShiftKey);
            IsRShiftPressed = Check(Key, Win32.VirtualKey.RShift, Keys.RShiftKey);

            IsLWinPressed = Check(Key, Win32.VirtualKey.LWin, Keys.LWin);
            IsRWinPressed = Check(Key, Win32.VirtualKey.RWin, Keys.RWin);

            if (new[] { Keys.LMenu, Keys.RMenu, Keys.LControlKey, Keys.RControlKey, Keys.LShiftKey, Keys.RShiftKey, Keys.LWin, Keys.RWin }.Contains(Key))
            {
                Key = Keys.None;
            }
        }

        public override string ToString() => $"Key={Key}; Win={IsWinPressed}; Alt={IsAltPressed}; Ctrl={IsCtrlPressed}; Shift={IsShiftPressed}";
    }
}