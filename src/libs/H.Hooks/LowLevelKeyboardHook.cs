using System;

namespace H.NET.Utilities
{
    public class LowLevelKeyboardHook : Hook
    {
        #region Properties

        public bool OneUpEvent { get; set; } = true;

        private Tuple<int, int>? LastState { get; set; }

        #endregion

        #region Events

        public event EventHandler<KeyboardHookEventArgs>? KeyDown;
        public event EventHandler<KeyboardHookEventArgs>? KeyUp;

        #endregion

        #region Constructors

        public LowLevelKeyboardHook() : base("Low Level Keyboard Hook", Winuser.WH_KEYBOARD_LL)
        {
        }

        #endregion

        #region Protected methods

        protected override int InternalCallback(int code, int wParam, IntPtr lParamPtr)
        {
            if (code < 0)
            {
                return 0;
            }

            var lParam = ToStructure<Win32.KeyboardHookStruct>(lParamPtr);
            if (OneUpEvent)
            {
                if (LastState != null && LastState.Item1 == lParam.VirtualKeyCode && LastState.Item2 == lParam.Flags)
                {
                    return 0;
                }
                LastState = new Tuple<int, int>(lParam.VirtualKeyCode, lParam.Flags);
            }

            var isKeyDown = lParam.Flags >> 7 == 0;
            var e = new KeyboardHookEventArgs(lParam);
            if (isKeyDown)
            {
                KeyDown?.Invoke(this, e);
            }
            else
            {
                KeyUp?.Invoke(this, e);
            }

            return e.Handled ? -1 : 0;
        }

        #endregion

    }
}