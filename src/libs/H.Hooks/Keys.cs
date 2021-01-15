using System;
using System.Linq;

namespace H.Hooks
{
    public class Keys
    {
        #region Static methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static Keys Parse(string text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));

            return new Keys(text.Split('+')
                .Select(static value => value.Trim())
                .Select(KeyExtensions.Parse)
                .ToArray());
        }

        public static Keys FromSpecialData(int data)
        {
            var number = data - 16;

            return new Keys(number == 0 ? Key.XButton2 : Key.XButton1);
        }

        #endregion

        #region Properties

        public Key[] Values { get; }

        public bool IsRightCtrl => Values.Contains(Key.RControlKey);
        public bool IsRightAlt => Values.Contains(Key.RMenu);
        public bool IsRightShift => Values.Contains(Key.RShiftKey);

        public bool IsLeftCtrl => Values.Contains(Key.LControlKey);
        public bool IsLeftAlt => Values.Contains(Key.LMenu);
        public bool IsLeftShift => Values.Contains(Key.LShiftKey);

        public bool IsAlt => IsRightAlt || IsLeftAlt || Values.Contains(Key.Menu) || Values.Contains(Key.Alt);
        public bool IsCtrl => IsLeftCtrl || IsRightCtrl || Values.Contains(Key.ControlKey) || Values.Contains(Key.Control);
        public bool IsShift => IsRightShift || IsLeftShift || Values.Contains(Key.ShiftKey) || Values.Contains(Key.Shift);

        public bool IsEmpty => !Values.Any();

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public Keys(params Key[] values)
        {
            Values = values;
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return string.Join("+", Values.Select(value => $"{value:G}"));
        }

        public override bool Equals(object obj)
        {
            if (obj is not Keys other)
            {
                return false;
            }

            return Equals(other);
        }

        private bool Equals(Keys other)
        {
            return 
                Values.Length == other.Values.Length &&
                Values.All(value => other.Values.Contains(value));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Values
                    .Aggregate(27, static (current, value) => (current * 397) ^ value.GetHashCode());
            }
        }

        #endregion
    }
}
