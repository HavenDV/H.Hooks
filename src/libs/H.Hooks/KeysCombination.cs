using System;
using System.Linq;

namespace H.NET.Utilities
{
    public class KeysCombination
    {
        #region Static methods

        public static KeysCombination FromString(string text)
        {
            /*
            const string mousePrefix = "SPECIALMOUSEBUTTON";
            if (text.StartsWith(mousePrefix, StringComparison.OrdinalIgnoreCase))
            {
                var number = text.Replace(mousePrefix, string.Empty);
                return new KeysCombination(Keys.XButton1, false, false, false);
            }*/

            var values = text.Contains("+") ? text.Split('+') : new[] { text };

            var ctrl = values.Contains("CTRL", StringComparer.OrdinalIgnoreCase);
            var alt = values.Contains("ALT", StringComparer.OrdinalIgnoreCase);
            var shift = values.Contains("SHIFT", StringComparer.OrdinalIgnoreCase);
            var mainKey = values.FirstOrDefault(i => !new[] { "CTRL", "ALT", "SHIFT" }.Contains(i, StringComparer.OrdinalIgnoreCase));

            var key = Hook.FromString(mainKey);

            return new KeysCombination(key, ctrl, shift, alt);
        }

        public static KeysCombination FromSpecialData(int data)
        {
            var number = data - 16;

            return new KeysCombination(number == 0 ? Keys.XButton2 : Keys.XButton1);
        }

        #endregion

        #region Properties

        private Keys Key { get; }
        private bool IsCtrl { get; }
        private bool IsAlt { get; }
        private bool IsShift { get; }

        public bool IsEmpty => Key == Keys.None;

        #endregion

        #region Constructors

        public KeysCombination(Keys key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            Key = key;
            IsCtrl = ctrl;
            IsShift = shift;
            IsAlt = alt;
        }

        #endregion

        #region Public methods

        private static string ToString(bool value, string name) => value ? $"{name}+" : string.Empty;

        public override string ToString()
        {
            return $"{ToString(IsCtrl, "CTRL")}{ToString(IsShift, "SHIFT")}{ToString(IsAlt, "ALT")}{Key:G}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is KeysCombination other))
            {
                return false;
            }

            return Equals(other);
        }

        private bool Equals(KeysCombination other)
        {
            return Key == other.Key && IsCtrl == other.IsCtrl && IsAlt == other.IsAlt && IsShift == other.IsShift;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Key;
                hashCode = (hashCode * 397) ^ IsCtrl.GetHashCode();
                hashCode = (hashCode * 397) ^ IsAlt.GetHashCode();
                hashCode = (hashCode * 397) ^ IsShift.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}
