using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;

namespace H.Hooks;

/// <summary>
/// 
/// </summary>
#if NET5_0_OR_GREATER
[System.Runtime.Versioning.SupportedOSPlatform("windows5.0")]
#elif NETSTANDARD2_0_OR_GREATER || NET451_OR_GREATER
#else
#error Target Framework is not supported
#endif
public static class Keyboard
{
    #region Public methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="useKeyboardState"></param>
    /// <param name="isCapsLock"></param>
    /// <param name="isLeftRightGranularity"></param>
    /// <param name="isExtendedMode"></param>
    /// <returns></returns>
    public static unsafe IEnumerable<Key> GetPressedKeys(
        bool useKeyboardState = true,
        bool isCapsLock = false,
        bool isLeftRightGranularity = false,
        bool isExtendedMode = false)
    {
        var keys = new List<Key>();

        var buffer = (byte[]?)null;
        if (useKeyboardState)
        {
            // Updates internal buffer.
            _ = PInvoke.GetKeyState((int)Key.Shift);

            buffer = new byte[256];
            fixed (byte* bufferPtr = buffer)
            {
                _ = PInvoke.GetKeyboardState(bufferPtr).EnsureNonZero();
            }
        }

        CheckUpAndAdd(keys, Key.LWin, buffer);
        CheckUpAndAdd(keys, Key.RWin, buffer);

        CheckUpAndAdd(keys, Key.LAlt, buffer);
        CheckUpAndAdd(keys, Key.RAlt, buffer);

        CheckUpAndAdd(keys, Key.LCtrl, buffer);
        CheckUpAndAdd(keys, Key.RCtrl, buffer);

        CheckUpAndAdd(keys, Key.LShift, buffer);
        CheckUpAndAdd(keys, Key.RShift, buffer);

        if (isCapsLock)
        {
            CheckToggledAndAdd(keys, Key.Caps, buffer);
        }

        if (isLeftRightGranularity)
        {
            ReplaceIfExists(keys, Key.Alt, Key.LAlt);
            ReplaceIfExists(keys, Key.Ctrl, Key.LCtrl);
            ReplaceIfExists(keys, Key.Shift, Key.LShift);
        }
        else
        {
            ReplaceIfExists(keys, Key.LAlt, Key.Alt);
            ReplaceIfExists(keys, Key.RAlt, Key.Alt);
            ReplaceIfExists(keys, Key.LCtrl, Key.Ctrl);
            ReplaceIfExists(keys, Key.RCtrl, Key.Ctrl);
            ReplaceIfExists(keys, Key.LShift, Key.Shift);
            ReplaceIfExists(keys, Key.RShift, Key.Shift);
        }

        if (isExtendedMode)
        {
            //keys.AddRange(buffer
            //    .Select((@short, index) => new Tuple<short, int>(@short, index))
            //    .Where(pair => (pair.Item1 & KF.BYTE_UP) > 0)
            //    .Select(pair => (Key)pair.Item2));

            for (var key = Key.D0; key <= Key.D9; key++)
            {
                CheckUpAndAdd(keys, key, buffer);
            }
            for (var key = Key.A; key <= Key.Z; key++)
            {
                CheckUpAndAdd(keys, key, buffer);
            }
        }

        return keys;
    }

    #endregion

    #region Private methods

    private static void ReplaceIfExists(List<Key> keys, Key from, Key to)
    {
        if (!keys.Contains(from))
        {
            return;
        }

        keys.Remove(from);

        if (!keys.Contains(to))
        {
            keys.Add(to);
        }
    }

    private static void CheckAndAdd(ICollection<Key> keys, Key key, byte[]? buffer, int kf)
    {
        if (keys.Contains(key))
        {
            return;
        }

        var state = buffer?[(int)key] ?? PInvoke.GetKeyState((int)key);
        if (Convert.ToBoolean(state & kf))
        {
            keys.Add(key);
        }
    }

    private static void CheckUpAndAdd(ICollection<Key> keys, Key key, byte[]? buffer)
    {
        CheckAndAdd(keys, key, buffer, buffer != null ? KF.BYTE_UP : (int)PInvoke.KF_UP);
    }

    private static void CheckToggledAndAdd(ICollection<Key> keys, Key key, byte[]? buffer)
    {
        CheckAndAdd(keys, key, buffer, KF.TOGGLED);
    }

    #endregion
}
