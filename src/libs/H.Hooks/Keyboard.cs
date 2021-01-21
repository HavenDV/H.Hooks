using System;
using System.Collections.Generic;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
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
        public static IEnumerable<Key> GetPressedKeys(
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
                User32.GetKeyState((int)Key.Shift);

                buffer = new byte[256];
                User32.GetKeyboardState(buffer).Check();
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

        private static void ReplaceIfExists(ICollection<Key> keys, Key from, Key to)
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

            var state = buffer?[(int)key] ?? User32.GetKeyState((int)key);
            if (Convert.ToBoolean(state & kf))
            {
                keys.Add(key);
            }
        }

        private static void CheckUpAndAdd(ICollection<Key> keys, Key key, byte[]? buffer)
        {
            CheckAndAdd(keys, key, buffer, buffer != null ? KF.BYTE_UP : KF.UP);
        }

        private static void CheckToggledAndAdd(ICollection<Key> keys, Key key, byte[]? buffer)
        {
            CheckAndAdd(keys, key, buffer, KF.TOGGLED);
        }

        #endregion
    }
}
