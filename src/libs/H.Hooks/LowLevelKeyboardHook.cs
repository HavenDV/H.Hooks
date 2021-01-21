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
        /// Sends multiple events while key pressed. <br/>
        /// Default value: <see langword="true"/>.
        /// </summary>
        public bool OneUpEvent { get; set; } = true;

        /// <summary>
        /// Allows common key combinations, like 1 + 2 + 3. <br/>
        /// Default value: <see langword="false"/>.
        /// </summary>
        public bool IsExtendedMode { get; set; }

        /// <summary>
        /// Allows handle modifier keys. <br/>
        /// Default value: <see langword="false"/>.
        /// </summary>
        public bool HandleModifierKeys { get; set; }

        /// <summary>
        /// Events will contains separate Left/Right keys. <br/>
        /// Default value: <see langword="false"/>.
        /// </summary>
        public bool IsLeftRightGranularity { get; set; }

        /// <summary>
        /// Uses User32.GetKeyboardState instead User32.GetKeyState. <br/>
        /// Disable this if any problem. <br/>
        /// Default value: <see langword="true"/>.
        /// </summary>
        public bool UseKeyboardState { get; set; } = true;

        /// <summary>
        /// Adds <see cref="Key.Caps"/> to each event if CapsLock is toggled. <br/>
        /// Default value: <see langword="true"/>.
        /// </summary>
        public bool IsCapsLock { get; set; }

        private Tuple<uint, uint, uint, nint, bool>? LastState { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<KeyboardEventArgs>? Down;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<KeyboardEventArgs>? Up;

        private void OnDown(KeyboardEventArgs value)
        {
            Down?.Invoke(this, value, PushToThreadPool);
        }

        private void OnUp(KeyboardEventArgs value)
        {
            Up?.Invoke(this, value, PushToThreadPool);
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
        protected override bool InternalCallback(int nCode, nint wParam, nint lParam)
        {
            var value = InteropUtilities.ToStructure<KeyboardHookStruct>(lParam);
            if (OneUpEvent &&
                LastState != null &&
                LastState.Item1 == value.VirtualKeyCode &&
                LastState.Item2 == value.Flags &&
                LastState.Item3 == value.ScanCode &&
                LastState.Item4 == value.ExtraInfo)
            {
                return LastState.Item5;
            }

            var keys = new List<Key>();
            var mainKey = (Key)value.VirtualKeyCode;
            keys.Add(mainKey);

            var buffer = (byte[]?)null;
            if (UseKeyboardState)
            {
                // Updates internal buffer.
                User32.GetKeyState((int)Key.Shift);

                buffer = new byte[256];
                User32.GetKeyboardState(buffer).Check();
            }

            CheckUpAndAdd(keys, Key.LWin, buffer);
            CheckUpAndAdd(keys, Key.RWin, buffer);

            CheckUpAndAdd(keys, Key.LAlt, buffer);
            CheckUpAndAdd(keys, Key.RAlt, buffer);

            CheckUpAndAdd(keys, Key.LCtrl, buffer);
            CheckUpAndAdd(keys, Key.RCtrl, buffer);

            CheckUpAndAdd(keys, Key.LShift, buffer);
            CheckUpAndAdd(keys, Key.RShift, buffer);

            if (IsCapsLock)
            {
                CheckToggledAndAdd(keys, Key.Caps, buffer);
            }

            if (IsLeftRightGranularity)
            {
                ReplaceIfExists(keys, Key.Alt, Key.LAlt);
                ReplaceIfExists(keys, Key.Ctrl, Key.LCtrl);
                ReplaceIfExists(keys, Key.Shift, Key.LShift);
            }
            else
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
                //keys.AddRange(buffer
                //    .Select((@short, index) => new Tuple<short, int>(@short, index))
                //    .Where(pair => (pair.Item1 & KF.BYTE_UP) > 0)
                //    .Select(pair => (Key)pair.Item2));

                for (var key = Key.D0; key <= Key.D9; key++)
                {
                    CheckUpAndAdd(keys, key, buffer);
                }
                for (var key = Key.A; key <= Key.Z; key++)
                {
                    CheckUpAndAdd(keys, key, buffer);
                }
            }

            var args = new KeyboardEventArgs(keys.ToArray());
            var isKeyDown = value.Flags >> 7 == 0;
            if (isKeyDown)
            {
                OnDown(args);
            }
            else
            {
                OnUp(args);
            }

            var isHandled = args.IsHandled;

            // Disabling handling for modifier keys.
            // If you handle separate modifier keys, you can't handle combinations.
            if (!HandleModifierKeys &&
                keys.All(key => new[]
                {
                    Key.LCtrl, Key.LAlt, Key.LShift, Key.LWin,
                    Key.RCtrl, Key.RAlt, Key.RShift, Key.RWin,
                    Key.Ctrl, Key.Alt, Key.Shift,
                    Key.Caps,
                }.Contains(key)))
            {
                isHandled = false;
            }

            if (OneUpEvent)
            {
                LastState = Tuple.Create(
                    value.VirtualKeyCode,
                    value.Flags,
                    value.ScanCode,
                    value.ExtraInfo,
                    isHandled);
            }

            return isHandled;
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

        private static void CheckAndAdd(ICollection<Key> keys, Key key, byte[]? buffer, int kf)
        {
            if (keys.Contains(key))
            {
                return;
            }

            var state = buffer?[(int)key] ?? User32.GetKeyState((int)key);
            if (Convert.ToBoolean(state & kf))
            {
                keys.Add(key);
            }
        }

        private static void CheckUpAndAdd(ICollection<Key> keys, Key key, byte[]? buffer)
        {
            CheckAndAdd(keys, key, buffer, buffer != null ? KF.BYTE_UP : KF.UP);
        }

        private static void CheckToggledAndAdd(ICollection<Key> keys, Key key, byte[]? buffer)
        {
            CheckAndAdd(keys, key, buffer, KF.TOGGLED);
        }

        #endregion
    }
}