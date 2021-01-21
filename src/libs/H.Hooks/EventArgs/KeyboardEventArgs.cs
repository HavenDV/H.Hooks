using System;
// ReSharper disable CheckNamespace

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyboardEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Keys Keys { get; }

        /// <summary>
        /// Set this property to <see langword="true"/> inside
        /// your event handler to prevent further processing
        /// of the event in other applications.
        /// </summary>
        public bool IsHandled { get; set; }

        #endregion

        #region Constructors

        internal KeyboardEventArgs(params Key[] keys)
        {
            Keys = new Keys(keys);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Keys}";
        }

        #endregion
    }
}