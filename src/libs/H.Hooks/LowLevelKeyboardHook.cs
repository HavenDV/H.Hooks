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
        /// Allows handle modifier keys. <br/>
        /// Default value: <see langword="false"/>.
        /// </summary>
        public bool HandleModifierKeys { get; set; }

        /// <summary>
        /// Allows common key combinations, like 1 + 2 + 3. <br/>
        /// Default value: <see langword="false"/>.
        /// </summary>
        public bool IsExtendedMode { get; set; }

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

            keys.AddRange(
                Keyboard.GetPressedKeys(
                    UseKeyboardState, 
                    IsCapsLock, 
                    IsLeftRightGranularity, 
                    IsExtendedMode
                    )
            );

            var args = new KeyboardEventArgs(keys.Distinct().ToArray());
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

        #endregion
    }
}