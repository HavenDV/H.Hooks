using System.Runtime.InteropServices;

namespace H.Hooks.Core.Interop
{
    internal static class Kernel32
    {
        /// <summary>
        /// Retrieves the thread identifier of the calling thread.
        /// </summary>
        /// <returns>The return value is the thread identifier of the calling thread.</returns>
        /// <remarks>
        /// Until the thread terminates, the thread identifier uniquely identifies
        /// the thread throughout the system.
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetCurrentThreadId();
    }
}
