using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Allows common key combinations, like 1 + 2 + 3.
        /// </summary>
        public bool IsExtendedMode { get; set; }

        /// <summary>
        /// Allows handle modifier keys.
        /// </summary>
        public bool HandleModifierKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLeftRightGranularity { get; set; }

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

            var keys = new List<Key>();
            var mainKey = (Key)value.VirtualKeyCode;
            keys.Add(mainKey);

            CheckAndAdd(keys, Key.LWin);
            CheckAndAdd(keys, Key.RWin);

            CheckAndAdd(keys, Key.LAlt);
            CheckAndAdd(keys, Key.RAlt);

            CheckAndAdd(keys, Key.LCtrl);
            CheckAndAdd(keys, Key.RCtrl);

            CheckAndAdd(keys, Key.LShift);
            CheckAndAdd(keys, Key.RShift);

            if (!IsLeftRightGranularity)
            {
                ReplaceIfExists(keys, Key.LAlt, Key.Alt);
                ReplaceIfExists(keys, Key.RAlt, Key.Alt);
                ReplaceIfExists(keys, Key.LCtrl, Key.Ctrl);
                ReplaceIfExists(keys, Key.RCtrl, Key.Ctrl);
                ReplaceIfExists(keys, Key.LShift, Key.Shift);
                ReplaceIfExists(keys, Key.RShift, Key.Shift);
            }

            if (IsExtendedMode)
            {
                //User32.GetKeyState((int) Key.ShiftKey);
                //var buffer = new byte[256];
                //User32.GetKeyboardState(buffer);
                //keys.AddRange(buffer
                //    .Select((@byte, index) => (@byte, index))
                //    .Where(pair => (pair.@byte & (int)KeyFlag.Up) > 0)
                //    .Select(pair => (Key)pair.index));

                for (var key = Key.D0; key <= Key.D9; key++)
                {
                    CheckAndAdd(keys, key);
                }
                for (var key = Key.A; key <= Key.Z; key++)
                {
                    CheckAndAdd(keys, key);
                }
            }

            var args = new KeyboardHookEventArgs(keys.ToArray());
            var isKeyDown = value.Flags >> 7 == 0;
            if (isKeyDown)
            {
                OnKeyDown(args);
            }
            else
            {
                OnKeyUp(args);
            }

            // Disabling handling for modifier keys.
            // If you handle separate modifier keys, you can't handle combinations.
            if (!HandleModifierKeys &&
                keys.All(key => new[]
                {
                    Key.LCtrl, Key.LAlt, Key.LShift, Key.LWin,
                    Key.RCtrl, Key.RAlt, Key.RShift, Key.RWin,
                    Key.Ctrl, Key.Alt, Key.Shift,
                }.Contains(key)))
            {
                return false;
            }

            return args.IsHandled;
        }

        private static void ReplaceIfExists(ICollection<Key> keys, Key from, Key to)
        {
            if (!keys.Contains(from))
            {
                return;
            }

            keys.Remove(from);

            if (!keys.Contains(to))
            {
                keys.Add(to);
            }
        }

        private static void CheckAndAdd(ICollection<Key> keys, Key key)
        {
            if (keys.Contains(key))
            {
                return;
            }

            if (Convert.ToBoolean(User32.GetKeyState((int)key) & (int)KeyFlag.Up))
            {
                keys.Add(key);
            }
        }

        #endregion
    }
}