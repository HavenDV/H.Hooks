using System;
using System.Globalization;

namespace H.Hooks
{
    /// <summary>
    /// /
    /// </summary>
    public static class Registry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#elif NETSTANDARD2_0_OR_GREATER || NET451_OR_GREATER
#else
#error Target Framework is not supported
#endif
        public static TimeSpan GetDoubleClickSpeed()
        {
            using var key = Microsoft.Win32.Registry.CurrentUser
                .OpenSubKey(@"Control Panel\Mouse");
            var value = key?.GetValue("DoubleClickSpeed") as string ?? "500";

            return TimeSpan.FromMilliseconds(Convert.ToInt32(value, CultureInfo.InvariantCulture));
        }
    }
}
