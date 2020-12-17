using System;
using System.Runtime.InteropServices;

namespace WeVPN.Firewall
{
    public class MarshalPtr<T> : IDisposable
    {
        #region Properties

        public T Value { get; }
        public IntPtrWrapper IntPtrWrapper { get; }

        #endregion

        #region Constructors

        public MarshalPtr(T value, int size, Action<IntPtrWrapper> initializeAction)
        {
            Value = value;
            IntPtrWrapper = new IntPtrWrapper(
                Marshal.AllocHGlobal(size),
                Marshal.FreeHGlobal);

            initializeAction?.Invoke(IntPtrWrapper);
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            IntPtrWrapper.Dispose();
        }

        public static implicit operator IntPtr(MarshalPtr<T> guidPtr)
        {
            return guidPtr.IntPtrWrapper;
        }

        #endregion
    }
}
