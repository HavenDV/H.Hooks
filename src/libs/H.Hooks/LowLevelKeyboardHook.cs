using System;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;

namespace H.Hooks
{
    public class LowLevelKeyboardHook : Hook
    {
        #region Properties

        public bool OneUpEvent { get; set; } = true;

        private Tuple<uint, uint>? LastState { get; set; }

        #endregion

        #region Events

        public event EventHandler<KeyboardHookEventArgs>? KeyDown;
        public event EventHandler<KeyboardHookEventArgs>? KeyUp;

        #endregion

        #region Protected methods

        public void Start()
        {
            Start(HookProcedureType.KeyboardLowLevel);
        }

        protected override nint InternalCallback(int code, int wParam, nint lParamPtr)
        {
            if (code < 0)
            {
                return 0;
            }

            var lParam = InteropUtilities.ToStructure<KeyboardHookStruct>(lParamPtr);
            if (OneUpEvent)
            {
                if (LastState != null && LastState.Item1 == lParam.VirtualKeyCode && LastState.Item2 == lParam.Flags)
                {
                    return 0;
                }
                LastState = new Tuple<uint, uint>(lParam.VirtualKeyCode, lParam.Flags);
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