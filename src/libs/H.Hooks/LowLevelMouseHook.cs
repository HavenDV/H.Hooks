using System;
using System.Collections.Generic;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;
using H.Hooks.Extensions;

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LowLevelMouseHook : Hook
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool GenerateMouseMoveEvents { get; set; }

        /// <summary>
        /// Adding keyboard keys. Please see properties:
        /// IsExtendedMode/IsLeftRightGranularity/UseKeyboardState/IsCapsLock
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

        /// <summary>
        /// Default: Registry value HKCU\Control Panel\Mouse\DoubleClickSpeed or 500 ms.
        /// </summary>
        public TimeSpan DoubleClickSpeed { get; set; }

        private DateTime PreviousDownTime { get; set; } = DateTime.MinValue;
        private DateTime LastDownTime { get; set; } = DateTime.MinValue;

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
        /// 
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
        public LowLevelMouseHook() : base(WH.MOUSE_LL)
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
        protected override bool InternalCallback(int nCode, nint wParam, nint lParam)
        {
            if (wParam == WM.MOUSEMOVE &&
                !GenerateMouseMoveEvents)
            {
                return false;
            }

            var value = InteropUtilities.ToStructure<MouseLowLevelHookStruct>(lParam);
            
            //detect button clicked
            var key = Key.MouseNone;
            var mouseDelta = 0;
            var isDoubleClick = false;
            var mouseDown = false;
            var mouseUp = false;
            var mouseMove = false;
            var specialKey = (value.MouseData >> 16) switch
            {
                1 => Key.MouseXButton1,
                2 => Key.MouseXButton2,
                _ => Key.MouseNone,
            };

            switch (wParam)
            {
                case WM.LBUTTONDOWN:
                    mouseDown = true;
                    key = Key.MouseLeft;
                    break;
                case WM.LBUTTONUP:
                    mouseUp = true;
                    key = Key.MouseLeft;
                    break;
                case WM.LBUTTONDBLCLK:
                    key = Key.MouseLeft;
                    isDoubleClick = true;
                    break;

                case WM.RBUTTONDOWN:
                    mouseDown = true;
                    key = Key.MouseRight;
                    break;
                case WM.RBUTTONUP:
                    mouseUp = true;
                    key = Key.MouseRight;
                    break;
                case WM.RBUTTONDBLCLK:
                    key = Key.MouseRight;
                    isDoubleClick = true;
                    break;

                case WM.MBUTTONDOWN:
                    mouseDown = true;
                    key = Key.MouseMiddle;
                    break;
                case WM.MBUTTONUP:
                    mouseUp = true;
                    key = Key.MouseMiddle;
                    break;
                case WM.MBUTTONDBLCLK:
                    key = Key.MouseMiddle;
                    isDoubleClick = true;
                    break;

                case WM.XBUTTONDOWN:
                case WM.NCXBUTTONDOWN:
                    mouseDown = true;
                    key = specialKey;
                    break;
                case WM.XBUTTONUP:
                case WM.NCXBUTTONUP:
                    mouseUp = true;
                    key = specialKey;
                    break;
                case WM.XBUTTONDBLCLK:
                case WM.NCXBUTTONDBLCLK:
                    key = specialKey;
                    isDoubleClick = true;
                    break;

                case WM.MOUSEWHEEL:
                case WM.MOUSEWHEELALT:
                    mouseDelta = (short)((value.MouseData >> 16) & 0xffff);
                    key = Key.MouseWheel;
                    break;

                case WM.MOUSEMOVE:
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

            var args = new MouseEventArgs(
                value.Point.X,
                value.Point.Y,
                mouseDelta,
                isDoubleClick,
                keys.ToArray());

            if (mouseUp)
            {
                OnUp(args);
                if (DateTime.Now.Subtract(PreviousDownTime) < DoubleClickSpeed)
                {
                    OnDoubleClick(args);
                }
            }
            if (mouseDown)
            {
                PreviousDownTime = LastDownTime;
                LastDownTime = DateTime.Now;
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