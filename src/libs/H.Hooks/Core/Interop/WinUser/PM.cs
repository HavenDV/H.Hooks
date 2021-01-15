// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
namespace H.Hooks.Core.Interop.WinUser
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-peekmessagea#parameters
    /// </summary>
    internal static class PM
    {
        /// <summary>
        /// Messages are not removed from the queue after processing by PeekMessage.
        /// </summary>
        public const uint NOREMOVE = 0x0000;

        /// <summary>
        /// Messages are removed from the queue after processing by PeekMessage.
        /// </summary>
        public const uint REMOVE = 0x0001;

        /// <summary>
        /// Prevents the system from releasing any thread that is waiting
        /// for the caller to go idle (see WaitForInputIdle). <br/>
        /// Combine this value with either PM_NOREMOVE or PM_REMOVE.
        /// </summary>
        public const uint NOYIELD = 0x0002;
    }
}
