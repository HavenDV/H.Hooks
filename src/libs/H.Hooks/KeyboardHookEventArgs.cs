using System;
using System.Linq;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;

namespace H.Hooks
{
    public class KeyboardHookEventArgs
    {
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

        private static bool Check(Keys key, VirtualKey virtualKey, Keys realKey) =>
            Convert.ToBoolean(User32.GetKeyState(virtualKey) & (int)KeyFlag.Up) || 
            key == realKey;

        internal KeyboardHookEventArgs(KeyboardHookStruct lParam)
        {
            Key = (Keys)lParam.VirtualKeyCode;

            //Control.ModifierKeys doesn't capture alt/win, and doesn't have r/l granularity
            IsLAltPressed = Check(Key, VirtualKey.LeftAlt, Keys.LMenu);
            IsRAltPressed = Check(Key, VirtualKey.RightAlt, Keys.RMenu);

            IsLCtrlPressed = Check(Key, VirtualKey.LeftControl, Keys.LControlKey);
            IsRCtrlPressed = Check(Key, VirtualKey.RightControl, Keys.RControlKey);

            IsLShiftPressed = Check(Key, VirtualKey.LeftShift, Keys.LShiftKey);
            IsRShiftPressed = Check(Key, VirtualKey.RightShift, Keys.RShiftKey);

            IsLWinPressed = Check(Key, VirtualKey.LeftWin, Keys.LWin);
            IsRWinPressed = Check(Key, VirtualKey.RightWin, Keys.RWin);

            if (new[] { Keys.LMenu, Keys.RMenu, Keys.LControlKey, Keys.RControlKey, Keys.LShiftKey, Keys.RShiftKey, Keys.LWin, Keys.RWin }.Contains(Key))
            {
                Key = Keys.None;
            }
        }

        public override string ToString() => $"Key={Key}; Win={IsWinPressed}; Alt={IsAltPressed}; Ctrl={IsCtrlPressed}; Shift={IsShiftPressed}";
    }
}