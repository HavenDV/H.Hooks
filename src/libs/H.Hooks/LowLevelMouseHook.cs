using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;
using H.Hooks.Extensions;

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows5.1.2600")]
#elif NETSTANDARD2_0_OR_GREATER || NET451_OR_GREATER
#else
#error Target Framework is not supported
#endif
    public sealed class LowLevelMouseHook : Hook
    {
        #region Properties

        /// <summary>
        /// Enables <see cref="Move"/> events. <br/>
        /// Default value: <see langword="false"/>.
        /// </summary>
        public bool GenerateMouseMoveEvents { get; set; }

        /// <summary>
        /// Default value: Registry value HKCU\Control Panel\Mouse\DoubleClickSpeed or 500 ms.
        /// </summary>
        public TimeSpan DoubleClickSpeed { get; set; }

        /// <summary>
        /// Adds keyboard keys. Allows getting combinations like Shift + LeftMouse. <br/>
        /// Please see properties:
        /// IsExtendedMode/IsLeftRightGranularity/UseKeyboardState/IsCapsLock. <br/>
        /// Default value: <see langword="false"/>.
        /// </summary>
        public bool AddKeyboardKeys { get; set; }

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

        private Dictionary<Key, DateTime> PreviousDownTimeDictionary { get; } = new();
        private Dictionary<Key, DateTime> LastDownTimeDictionary { get; } = new();

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs>? Up;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs>? Down;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs>? DoubleClick;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventArgs>? Wheel;

        /// <summary>
        /// Disabled by default. See <see cref="GenerateMouseMoveEvents"/>.
        /// </summary>
        public event EventHandler<MouseEventArgs>? Move;

        private void OnUp(MouseEventArgs value)
        {
            Up?.Invoke(this, value, PushToThreadPool);
        }

        private void OnDown(MouseEventArgs value)
        {
            Down?.Invoke(this, value, PushToThreadPool);
        }

        private void OnDoubleClick(MouseEventArgs value)
        {
            DoubleClick?.Invoke(this, value, PushToThreadPool);
        }

        private void OnWheel(MouseEventArgs value)
        {
            Wheel?.Invoke(this, value, PushToThreadPool);
        }

        private void OnMove(MouseEventArgs value)
        {
            Move?.Invoke(this, value, PushToThreadPool);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public LowLevelMouseHook() : base(WINDOWS_HOOK_ID.WH_MOUSE_LL)
        {
            DoubleClickSpeed = Registry.GetDoubleClickSpeed();
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
        internal override bool InternalCallback(int nCode, WPARAM wParam, LPARAM lParam)
        {
            if ((uint)wParam.Value == PInvoke.WM_MOUSEMOVE &&
                !GenerateMouseMoveEvents)
            {
                return false;
            }

            var value = InteropUtilities.ToStructure<MSLLHOOKSTRUCT>(lParam);
            
            //detect button clicked
            var key = Key.MouseNone;
            var mouseDelta = 0;
            var isDoubleClick = false;
            var mouseDown = false;
            var mouseUp = false;
            var mouseMove = false;
            //var specialKey = Key.MouseNone;
            //if (value.mouseData.HasFlag(MOUSEHOOKSTRUCTEX_MOUSE_DATA.XBUTTON1))
            //{
            //    specialKey = Key.MouseXButton1;
            //}
            //else if (value.mouseData.HasFlag(MOUSEHOOKSTRUCTEX_MOUSE_DATA.XBUTTON2))
            //{
            //    specialKey = Key.MouseXButton2;
            //}
            var specialKey = (((uint)value.mouseData) >> 16) switch
            {
                1 => Key.MouseXButton1,
                2 => Key.MouseXButton2,
                _ => Key.MouseNone,
            };
            
            switch ((uint)wParam.Value)
            {
                case PInvoke.WM_LBUTTONDOWN:
                    mouseDown = true;
                    key = Key.MouseLeft;
                    break;
                case PInvoke.WM_LBUTTONUP:
                    mouseUp = true;
                    key = Key.MouseLeft;
                    break;
                case PInvoke.WM_LBUTTONDBLCLK:
                    key = Key.MouseLeft;
                    isDoubleClick = true;
                    break;

                case PInvoke.WM_RBUTTONDOWN:
                    mouseDown = true;
                    key = Key.MouseRight;
                    break;
                case PInvoke.WM_RBUTTONUP:
                    mouseUp = true;
                    key = Key.MouseRight;
                    break;
                case PInvoke.WM_RBUTTONDBLCLK:
                    key = Key.MouseRight;
                    isDoubleClick = true;
                    break;

                case PInvoke.WM_MBUTTONDOWN:
                    mouseDown = true;
                    key = Key.MouseMiddle;
                    break;
                case PInvoke.WM_MBUTTONUP:
                    mouseUp = true;
                    key = Key.MouseMiddle;
                    break;
                case PInvoke.WM_MBUTTONDBLCLK:
                    key = Key.MouseMiddle;
                    isDoubleClick = true;
                    break;

                case PInvoke.WM_XBUTTONDOWN:
                case PInvoke.WM_NCXBUTTONDOWN:
                    mouseDown = true;
                    key = specialKey;
                    break;
                case PInvoke.WM_XBUTTONUP:
                case PInvoke.WM_NCXBUTTONUP:
                    mouseUp = true;
                    key = specialKey;
                    break;
                case PInvoke.WM_XBUTTONDBLCLK:
                case PInvoke.WM_NCXBUTTONDBLCLK:
                    key = specialKey;
                    isDoubleClick = true;
                    break;

                case PInvoke.WM_MOUSEWHEEL:
                case PInvoke.WM_MOUSEHWHEEL:
                    mouseDelta = (short)((((uint)value.mouseData) >> 16) & 0xffff);
                    key = Key.MouseWheel;
                    break;

                case PInvoke.WM_MOUSEMOVE:
                    mouseMove = true;
                    break;

                default:
                    mouseDown = true;
                    break;
            }

            var keys = new List<Key>();
            if (key != Key.MouseNone)
            {
                keys.Add(key);
            }

            if (AddKeyboardKeys)
            {
                keys.AddRange(
                    Keyboard.GetPressedKeys(
                        UseKeyboardState,
                        IsCapsLock,
                        IsLeftRightGranularity,
                        IsExtendedMode
                    )
                );
            }

            var newKeys = new Keys(keys.ToArray());
            var args = new MouseEventArgs(
                value.pt.x,
                value.pt.y,
                mouseDelta,
                isDoubleClick,
                newKeys,
                key);

            if (mouseUp)
            {
                OnUp(args);
                PreviousDownTimeDictionary
                    .TryGetValue(key, out var previousDateTime);
                if (DateTime.Now.Subtract(previousDateTime) < DoubleClickSpeed)
                {
                    OnDoubleClick(args);
                }
            }
            if (mouseDown)
            {
                LastDownTimeDictionary.TryGetValue(key, out var lastDownTime);
                PreviousDownTimeDictionary[key] = lastDownTime;
                LastDownTimeDictionary[key] = DateTime.Now;
                OnDown(args);
            }
            if (isDoubleClick)
            {
                OnDoubleClick(args);
            }
            if (mouseDelta != 0)
            {
                OnWheel(args);
            }
            if (mouseMove)
            {
                OnMove(args);
            }

            return args.IsHandled;
        }

        #endregion
    }
}