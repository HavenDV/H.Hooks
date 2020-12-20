using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop.WinUser
{
    /// <summary>
    /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
    /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.10240.0/um/WinUser.h#L1112 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseLowLevelHookStruct
    {
        /// <summary>
        /// Specifies a Point structure that contains the X- and Y-coordinates of the cursor, in screen coordinates. 
        /// </summary>
        public Point Point;
        
        /// <summary>
        /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. 
        /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward, 
        /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user. 
        /// One wheel click is defined as WHEEL_DELTA, which is 120. 
        ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
        /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
        /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, MouseData is not used. 
        ///XBUTTON1
        ///The first X button was pressed or released.
        ///XBUTTON2
        ///The second X button was pressed or released.
        /// </summary>
        public uint MouseData;
        
        /// <summary>
        /// Specifies the event-injected flag. An application can use the following value to test the mouse Flags. Value Purpose 
        ///LLMHF_INJECTED Test the event-injected flag.  
        ///0
        ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
        ///1-15
        ///Reserved.
        /// </summary>
        public uint Flags;
        
        /// <summary>
        /// Specifies the Time stamp for this message.
        /// </summary>
        public uint Time;
        
        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public nint ExtraInfo;
    }
}