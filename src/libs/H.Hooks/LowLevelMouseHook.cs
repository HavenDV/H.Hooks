using System;
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
        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventExtArgs>? MouseUp;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventExtArgs>? MouseDown;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventExtArgs>? MouseClick;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventExtArgs>? MouseClickExt;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventExtArgs>? MouseDoubleClick;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventExtArgs>? MouseWheel;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MouseEventExtArgs>? MouseMove;

        private void OnMouseUp(MouseEventExtArgs value)
        {
            MouseUp?.Invoke(this, value, PushToThreadPool);
        }

        private void OnMouseDown(MouseEventExtArgs value)
        {
            MouseDown?.Invoke(this, value, PushToThreadPool);
        }

        private void OnMouseClick(MouseEventExtArgs value)
        {
            MouseClick?.Invoke(this, value, PushToThreadPool);
        }

        private void OnMouseClickExt(MouseEventExtArgs value)
        {
            MouseClickExt?.Invoke(this, value, PushToThreadPool);
        }

        private void OnMouseDoubleClick(MouseEventExtArgs value)
        {
            MouseDoubleClick?.Invoke(this, value, PushToThreadPool);
        }

        private void OnMouseWheel(MouseEventExtArgs value)
        {
            MouseWheel?.Invoke(this, value, PushToThreadPool);
        }

        private void OnMouseMove(MouseEventExtArgs value)
        {
            MouseMove?.Invoke(this, value, PushToThreadPool);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public LowLevelMouseHook() : base(WH.MOUSE_LL)
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
            var value = InteropUtilities.ToStructure<MouseLowLevelHookStruct>(lParam);

            //detect button clicked
            var button = MouseButtons.None;
            short mouseDelta = 0;
            var clickCount = 0;
            var mouseDown = false;
            var mouseUp = false;
            var mouseMove = false;

            switch (wParam)
            {
                case WM.LBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case WM.LBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case WM.LBUTTONDBLCLK:
                    button = MouseButtons.Left;
                    clickCount = 2;
                    break;
                case WM.RBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case WM.RBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case WM.RBUTTONDBLCLK:
                    button = MouseButtons.Right;
                    clickCount = 2;
                    break;
                case WM.XBUTTONDOWN:
                case WM.NCXBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.XButton1;
                    clickCount = 1;
                    break;
                case WM.XBUTTONUP:
                case WM.NCXBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.XButton1;
                    clickCount = 1;
                    break;
                case WM.XBUTTONDBLCLK:
                case WM.NCXBUTTONDBLCLK:
                    button = MouseButtons.XButton1;
                    clickCount = 2;
                    break;
                case WM.MOUSEWHEEL:
                    //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
                    //One wheel click is defined as WHEEL_DELTA, which is 120. 
                    //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                    mouseDelta = (short)((value.MouseData >> 16) & 0xffff);

                    //TODO: X BUTTONS (I havent them so was unable to test)
                    //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                    //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                    //and the low-order word is reserved. This value can be one or more of the following values. 
                    //Otherwise, MouseData is not used. 
                    break;

                case WM.MOUSEMOVE:
                    mouseMove = true;
                    break;

                default:
                    mouseDown = true;
                    break;
            }

            //generate event 
            var args = new MouseEventExtArgs(
                button,
                clickCount,
                value.Point.X,
                value.Point.Y,
                mouseDelta);

            //Mouse up
            if (mouseUp)
            {
                OnMouseUp(args);
            }

            //Mouse down
            if (mouseDown)
            {
                args.SpecialButton = value.MouseData > 0
                    ? (int)Math.Log(value.MouseData, 2)
                    : 0;

                OnMouseDown(args);
            }

            //If someone listens to click and a click is heppened
            if (clickCount > 0)
            {
                OnMouseClick(args);
                OnMouseClickExt(args);
            }

            //If someone listens to double click and a click is heppened
            if (clickCount == 2)
            {
                OnMouseDoubleClick(args);
            }

            //Wheel was moved
            if (mouseDelta != 0)
            {
                OnMouseWheel(args);
            }

            //If someone listens to move and there was a change in coordinates raise move event
            //if (m_OldX != mouseHookStruct.Point.X || m_OldY != mouseHookStruct.Point.Y)
            if (mouseMove)
            {
                //m_OldX = mouseHookStruct.Point.X;
                //m_OldY = mouseHookStruct.Point.Y;

                OnMouseMove(args);

                //if (s_MouseMoveExt != null)
                //{
                //    s_MouseMoveExt.Invoke(null, e);
                //}
            }

            return args.Handled;
        }

        #endregion
    }
}