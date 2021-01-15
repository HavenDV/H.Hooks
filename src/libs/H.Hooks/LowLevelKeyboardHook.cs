using System;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;
using H.Hooks.Extensions;

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LowLevelKeyboardHook : Hook
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool OneUpEvent { get; set; } = true;

        private Tuple<uint, uint>? LastState { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<KeyboardHookEventArgs>? KeyDown;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<KeyboardHookEventArgs>? KeyUp;

        private void OnKeyDown(KeyboardHookEventArgs value)
        {
            KeyDown?.Invoke(this, value, PushToThreadPool);
        }

        private void OnKeyUp(KeyboardHookEventArgs value)
        {
            KeyUp?.Invoke(this, value, PushToThreadPool);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public LowLevelKeyboardHook() : base(WH.KEYBOARD_LL)
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParamPtr"></param>
        /// <returns></returns>
        protected override bool InternalCallback(int code, int wParam, nint lParamPtr)
        {
            var lParam = InteropUtilities.ToStructure<KeyboardHookStruct>(lParamPtr);
            if (OneUpEvent)
            {
                if (LastState != null && LastState.Item1 == lParam.VirtualKeyCode && LastState.Item2 == lParam.Flags)
                {
                    return true;
                }
                LastState = new Tuple<uint, uint>(lParam.VirtualKeyCode, lParam.Flags);
            }

            var args = new KeyboardHookEventArgs(lParam);
            var isKeyDown = lParam.Flags >> 7 == 0;
            if (isKeyDown)
            {
                OnKeyDown(args);
            }
            else
            {
                OnKeyUp(args);
            }

            return args.Handled;
        }

        #endregion
    }
}