using System;

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyboardHookEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Keys Keys { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHandled { get; set; }

        #endregion

        #region Constructors

        internal KeyboardHookEventArgs(params Key[] keys)
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