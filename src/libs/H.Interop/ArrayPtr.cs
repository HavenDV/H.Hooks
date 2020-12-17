using System;
using System.Runtime.InteropServices;

namespace WeVPN.Firewall
{
    public class ArrayPtr<T> : IDisposable
    {
        #region Properties

        public T[]? Values { get; }
        public IntPtrWrapper IntPtrWrapper { get; }

        #endregion

        #region Constructors

        public ArrayPtr(T[]? values)
        {
            Values = values;
            if (values == null || values.Length == 0)
            {
                IntPtrWrapper = new IntPtrWrapper();
                return;
            }

            IntPtrWrapper = new IntPtrWrapper(
                Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)) * values.Length),
                Marshal.FreeHGlobal);

            var longPtr = IntPtrWrapper.IntPtr.ToInt64();
            for (var i = 0; i < values.Length; i++)
            {
                var offsetPtr = new IntPtr(longPtr + i * Marshal.SizeOf(typeof(T)));

                Marshal.StructureToPtr(values[i], offsetPtr, true);
            }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            IntPtrWrapper.Dispose();
        }

        public static implicit operator IntPtr(ArrayPtr<T> ptr)
        {
            return ptr.IntPtrWrapper.IntPtr;
        }

        #endregion
    }
}
