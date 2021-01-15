using System;
using System.Collections.Generic;
using System.Linq;

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Keys FromSpecialData(int data)
        {
            var number = data - 16;

            return new Keys(number == 0 ? Key.XButton2 : Key.XButton1);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<Key> Values { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRightCtrl => Values.Contains(Key.RControlKey);

        /// <summary>
        /// 
        /// </summary>
        public bool IsRightAlt => Values.Contains(Key.RMenu);

        /// <summary>
        /// 
        /// </summary>
        public bool IsRightShift => Values.Contains(Key.RShiftKey);

        /// <summary>
        /// 
        /// </summary>
        public bool IsLeftCtrl => Values.Contains(Key.LControlKey);

        /// <summary>
        /// 
        /// </summary>
        public bool IsLeftAlt => Values.Contains(Key.LMenu);

        /// <summary>
        /// 
        /// </summary>
        public bool IsLeftShift => Values.Contains(Key.LShiftKey);
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsAlt => IsRightAlt || IsLeftAlt || Values.Contains(Key.Menu) || Values.Contains(Key.Alt);

        /// <summary>
        /// 
        /// </summary>
        public bool IsCtrl => IsLeftCtrl || IsRightCtrl || Values.Contains(Key.ControlKey) || Values.Contains(Key.Control);

        /// <summary>
        /// 
        /// </summary>
        public bool IsShift => IsRightShift || IsLeftShift || Values.Contains(Key.ShiftKey) || Values.Contains(Key.Shift);

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join("+", Values.Select(value => $"{value:G}"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
                Values.Count == other.Values.Count &&
                Values.All(value => other.Values.Contains(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
