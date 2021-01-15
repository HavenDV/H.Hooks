using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop.WinUser
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeMessage
    {
        public nint handle;
        public uint msg;
        public nint wParam;
        public nint lParam;
        public uint time;
        public Point p;
    }
}