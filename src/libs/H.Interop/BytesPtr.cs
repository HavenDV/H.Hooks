using System.Runtime.InteropServices;

namespace WeVPN.Firewall
{
    public class BytesPtr : MarshalPtr<byte[]>
    {
        #region Constructors

        public BytesPtr(byte[] value) : base(
            value, 
            value.Length, 
            wrapper => Marshal.Copy(value, 0, wrapper, value.Length))
        {
        }

        #endregion
    }
}
