using System;

namespace WeVPN.Firewall
{
    public class IntPtrWrapper : IDisposable
    {
        #region Properties

        public IntPtr IntPtr { get; private set; }
        private Action<IntPtr>? DisposeAction { get; }

        public bool IsEmpty => IntPtr == IntPtr.Zero;

        #endregion

        #region Constructors

        public IntPtrWrapper()
        {
            IntPtr = IntPtr.Zero;
        }

        public IntPtrWrapper(IntPtr intPtr, Action<IntPtr> disposeAction)
        {
            IntPtr = intPtr;
            DisposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Throws exception if not.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void EnsureIntPtrNotZero()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("IntPtr is zero");
            }
        }

        public void Dispose()
        {
            if (IsEmpty)
            {
                return;
            }

            DisposeAction?.Invoke(IntPtr);
            IntPtr = IntPtr.Zero;
        }

        public static implicit operator IntPtr(IntPtrWrapper wrapper)
        {
            wrapper.EnsureIntPtrNotZero();

            return wrapper.IntPtr;
        }

        #endregion
    }
}
