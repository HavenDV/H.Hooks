using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop.WinUser
{
    /// <summary>
    /// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
    /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.10240.0/um/WinUser.h#L1101
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyboardHookStruct
    {
        /// <summary>
        /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
        /// </summary>
        public uint VirtualKeyCode;
        
        /// <summary>
        /// Specifies a hardware scan code for the key. 
        /// </summary>
        public uint ScanCode;
        
        /// <summary>
        /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
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