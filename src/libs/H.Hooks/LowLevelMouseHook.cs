using System;
using H.Hooks.Core.Interop.WinUser;

namespace H.Hooks
{
    public class LowLevelMouseHook : Hook
    {
        #region Events

        public event EventHandler<MouseEventExtArgs>? MouseUp;
        public event EventHandler<MouseEventExtArgs>? MouseDown;
        public event EventHandler<MouseEventExtArgs>? MouseClick;
        public event EventHandler<MouseEventExtArgs>? MouseClickExt;
        public event EventHandler<MouseEventExtArgs>? MouseDoubleClick;
        public event EventHandler<MouseEventExtArgs>? MouseWheel;
        public event EventHandler<MouseEventExtArgs>? MouseMove;

        #endregion

        #region Constructors

        public LowLevelMouseHook() : base(nameof(LowLevelMouseHook))
        {
        }

        #endregion

        #region Protected methods

        public void Start()
        {
            Start(HookProcedureType.MouseLowLevel);
        }
        
        protected override IntPtr InternalCallback(int nCode, int wParam, IntPtr lParamPtr)
        {
            if (nCode < 0)
            {
                return IntPtr.Zero;
            }

            var lParam = ToStructure<MouseLowLevelHookStruct>(lParamPtr);

            //detect button clicked
            MouseButtons button = MouseButtons.None;
            short mouseDelta = 0;
            int clickCount = 0;
            var mouseDown = false;
            var mouseUp = false;
            var mouseMove = false;

            switch (wParam)
            {
                case WindowsMessages.WM_LBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case WindowsMessages.WM_LBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case WindowsMessages.WM_LBUTTONDBLCLK:
                    button = MouseButtons.Left;
                    clickCount = 2;
                    break;
                case WindowsMessages.WM_RBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case WindowsMessages.WM_RBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case WindowsMessages.WM_RBUTTONDBLCLK:
                    button = MouseButtons.Right;
                    clickCount = 2;
                    break;
                case WindowsMessages.WM_XBUTTONDOWN:
                case WindowsMessages.WM_NCXBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.XButton1;
                    clickCount = 1;
                    break;
                case WindowsMessages.WM_XBUTTONUP:
                case WindowsMessages.WM_NCXBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.XButton1;
                    clickCount = 1;
                    break;
                case WindowsMessages.WM_XBUTTONDBLCLK:
                case WindowsMessages.WM_NCXBUTTONDBLCLK:
                    button = MouseButtons.XButton1;
                    clickCount = 2;
                    break;
                case WindowsMessages.WM_MOUSEWHEEL:
                    //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
                    //One wheel click is defined as WHEEL_DELTA, which is 120. 
                    //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                    mouseDelta = (short)((lParam.MouseData >> 16) & 0xffff);

                    //TODO: X BUTTONS (I havent them so was unable to test)
                    //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                    //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                    //and the low-order word is reserved. This value can be one or more of the following values. 
                    //Otherwise, MouseData is not used. 
                    break;

                case WindowsMessages.WM_MOUSEMOVE:
                    mouseMove = true;
                    break;

                default:
                    mouseDown = true;
                    break;
            }

            //generate event 
            var e = new MouseEventExtArgs(
                                               button,
                                               clickCount,
                                               lParam.Point.X,
                                               lParam.Point.Y,
                                               mouseDelta);

            //Mouse up
            if (mouseUp)
            {
                MouseUp?.Invoke(null, e);
            }

            //Mouse down
            if (mouseDown)
            {
                e.SpecialButton = lParam.MouseData > 0 ?
                    (int)Math.Log(lParam.MouseData, 2) : 0;
                MouseDown?.Invoke(null, e);
            }

            //If someone listens to click and a click is heppened
            if (clickCount > 0)
            {
                MouseClick?.Invoke(null, e);
                MouseClickExt?.Invoke(null, e);
            }

            //If someone listens to double click and a click is heppened
            if (clickCount == 2)
            {
                MouseDoubleClick?.Invoke(null, e);
            }

            //Wheel was moved
            if (mouseDelta != 0)
            {
                MouseWheel?.Invoke(null, e);
            }
            
            //If someone listens to move and there was a change in coordinates raise move event
            //if (m_OldX != mouseHookStruct.Point.X || m_OldY != mouseHookStruct.Point.Y)
            if (mouseMove)
            {
                //m_OldX = mouseHookStruct.Point.X;
                //m_OldY = mouseHookStruct.Point.Y;
                
                MouseMove?.Invoke(null, e);

                //if (s_MouseMoveExt != null)
                //{
                //    s_MouseMoveExt.Invoke(null, e);
                //}
            }

            return e.Handled ? new IntPtr(-1) : IntPtr.Zero;
        }

        #endregion
    }
}