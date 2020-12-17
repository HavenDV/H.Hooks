using System.Runtime.InteropServices;

namespace WeVPN.Firewall
{
    public class StructurePtr<T> : MarshalPtr<T>
    {
        #region Constructors

        public StructurePtr(T value) : base(
            value, 
            Marshal.SizeOf(value),
            wrapper => Marshal.StructureToPtr(value, wrapper, false))
        {
        }

        #endregion
    }
}
