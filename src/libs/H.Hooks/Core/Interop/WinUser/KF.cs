// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
namespace H.Hooks.Core.Interop.WinUser
{
    /// <summary>
    /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.10240.0/um/WinUser.h#L424
    /// </summary>
    internal static class KF
    {
        /// <summary>
        /// 
        /// </summary>
        public const int TOGGLED = 0x0001;

        /// <summary>
        /// 
        /// </summary>
        public const int BYTE_UP = 0x80;

        /// <summary>
        /// 
        /// </summary>
        public const int EXTENDED = 0x0100;

        /// <summary>
        /// 
        /// </summary>
        public const int DLGMODE = 0x0800;

        /// <summary>
        /// 
        /// </summary>
        public const int MENUMODE = 0x1000;

        /// <summary>
        /// 
        /// </summary>
        public const int ALTDOWN = 0x2000;

        /// <summary>
        /// 
        /// </summary>
        public const int REPEAT = 0x4000;

        /// <summary>
        /// 
        /// </summary>
        public const int UP = 0x8000;
    }
}