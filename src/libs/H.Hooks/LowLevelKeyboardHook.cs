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
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        protected override bool InternalCallback(int nCode, int wParam, nint lParam)
        {
            var value = InteropUtilities.ToStructure<KeyboardHookStruct>(lParam);
            if (OneUpEvent)
            {
                if (LastState != null && LastState.Item1 == value.VirtualKeyCode && LastState.Item2 == value.Flags)
                {
                    return true;
                }
                LastState = new Tuple<uint, uint>(value.VirtualKeyCode, value.Flags);
            }

            var args = new KeyboardHookEventArgs(value);
            var isKeyDown = value.Flags >> 7 == 0;
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