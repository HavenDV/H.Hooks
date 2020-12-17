using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace WeVPN.Firewall
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/fwp/using-windows-filtering-platform
    /// </summary>
    // ReSharper disable once PartialTypeWithSinglePart
    internal partial class NativeMethods
    {
        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmEngineOpen0")]
        public static extern uint FwpmEngineOpen0(
            [In]
            [MarshalAs(UnmanagedType.LPWStr)] 
            string? serverName, 
            uint authnService,
            IntPtr authIdentity,
            ref FWPM_SESSION0 session, 
            out IntPtr engineHandle);

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmEngineClose0")]
        public static extern uint FwpmEngineClose0(IntPtr engineHandle);

        [DllImport("FWPUClnt.dll", EntryPoint = "FwpmSubLayerAdd0")]
        public static extern uint FwpmSubLayerAdd0(
            IntPtr engineHandle,
            ref FWPM_SUBLAYER0_ subLayer,
            IntPtr sd);

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmTransactionBegin0")]
        public static extern uint FwpmTransactionBegin0(
            IntPtr engineHandle,
            uint flags);

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmTransactionAbort0")]
        public static extern uint FwpmTransactionAbort0(
            IntPtr engineHandle);

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmTransactionCommit0")]
        public static extern uint FwpmTransactionCommit0(
            IntPtr engineHandle);

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmProviderContextAdd0")]
        public static extern uint FwpmProviderContextAdd0(
            IntPtr engineHandle,
            ref FWPM_PROVIDER_CONTEXT0 session,
            IntPtr sd,
            ref ulong id
            );

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmProviderAdd0")]
        public static extern uint FwpmProviderAdd0(
            IntPtr engineHandle,
            ref FWPM_PROVIDER0 provider,
            IntPtr sd
        );

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmCalloutAdd0")]
        public static extern uint FwpmCalloutAdd0(
            IntPtr engineHandle,
            ref FWPM_CALLOUT0 callout,
            IntPtr sd,
            ref uint id
        );

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmFilterAdd0")]
        public static extern uint FwpmFilterAdd0(
            IntPtr engineHandle, 
            ref FWPM_FILTER0_ filter, 
            IntPtr sd, 
            ref ulong id);

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmGetAppIdFromFileName0")]
        public static extern uint FwpmGetAppIdFromFileName0(
            [In]
            [MarshalAs(UnmanagedType.LPWStr)]
            string fileName,
            out IntPtr appId);

        [DllImport("Fwpuclnt.dll", EntryPoint = "FwpmFreeMemory0")]
        public static extern void FwpmFreeMemory0(
            in IntPtr p);
    }

    // ReSharper disable once PartialTypeWithSinglePart
    internal partial class NativeConstants
    {
        public static Guid cFWPM_CONDITION_IP_LOCAL_INTERFACE { get; } = new Guid("4cd62a49-59c3-4969-b7f3-bda5d32890a4");
        public static Guid cFWPM_CONDITION_IP_REMOTE_ADDRESS { get; } = new Guid("b235ae9a-1d64-49b8-a44c-5ff3d9095045");
        public static Guid cFWPM_CONDITION_IP_PROTOCOL { get; } = new Guid("3971ef2b-623e-4f9a-8cb1-6e79b806b9a7");
        public static Guid cFWPM_CONDITION_IP_LOCAL_PORT { get; } = new Guid("0c1ba1af-5765-453f-af22-a8f791ac775b");
        public static Guid cFWPM_CONDITION_IP_REMOTE_PORT { get; } = new Guid("c35a604d-d22b-4e1a-91b4-68f674ee674b");
        public static Guid cFWPM_CONDITION_ALE_APP_ID { get; } = new Guid("d78e1e87-8644-4ea5-9437-d809ecefc971");
        public static Guid cFWPM_CONDITION_ALE_USER_ID { get; } = new Guid("af043a0a-b34d-4f86-979c-c90371af6e66");
        public static Guid cFWPM_CONDITION_IP_LOCAL_ADDRESS { get; } = new Guid("d9ee00de-c1ef-4617-bfe3-ffd8f5a08957");
        public static Guid cFWPM_CONDITION_ICMP_TYPE { get; } = cFWPM_CONDITION_IP_LOCAL_PORT;
        public static Guid cFWPM_CONDITION_ICMP_CODE { get; } = cFWPM_CONDITION_IP_REMOTE_PORT;
        public static Guid cFWPM_CONDITION_L2_FLAGS { get; } = new Guid("7bc43cbf-37ba-45f1-b74a-82ff518eeb10");
        public static Guid FWPM_CONDITION_FLAGS { get; } = new Guid("632ce23b-5167-435c-86d7-e903684aa80c");

        public const uint cFWP_CONDITION_FLAG_IS_LOOPBACK = 0x00000001;

        public static Guid FWPM_LAYER_ALE_AUTH_CONNECT_V4 { get; } = new Guid("c38d57d1-05a7-4c33-904f-7fbceee60e82");
        public static Guid FWPM_LAYER_ALE_AUTH_RECV_ACCEPT_V4 { get; } = new Guid("e1cd9fe7-f4b5-4273-96c0-592e487b8650");
        public static Guid FWPM_LAYER_ALE_FLOW_ESTABLISHED_V4 { get; } = new Guid("af80470a-5596-4c13-9992-539e6fe57967");
        public static Guid FWPM_LAYER_ALE_AUTH_CONNECT_V6 { get; } = new Guid("4a72393b-319f-44bc-84c3-ba54dcb3b6b4");
        public static Guid cFWPM_LAYER_ALE_AUTH_RECV_ACCEPT_V6 { get; } = new Guid("a3b42c97-9f04-4672-b87e-cee9c483257f");
        public static Guid cFWPM_LAYER_OUTBOUND_MAC_FRAME_NATIVE { get; } = new Guid("94c44912-9d6f-4ebf-b995-05ab8a088d1b");
        public static Guid cFWPM_LAYER_INBOUND_MAC_FRAME_NATIVE { get; } = new Guid("d4220bd3-62ce-4f08-ae88-b56e8526df50");
        public static Guid cFWPM_LAYER_ALE_BIND_REDIRECT_V4 { get; } = new Guid("66978cad-c704-42ac-86ac-7c1a231bd253");
        public static Guid cFWPM_SUBLAYER_UNIVERSAL { get; } = new Guid("eebecc03-ced4-4380-819a-2734397b2b74");

        public const uint cFWPM_CALLOUT_FLAG_PERSISTENT = 0x00010000;
        public const uint cFWPM_CALLOUT_FLAG_USES_PROVIDER_CONTEXT = 0x00020000;
        public const uint cFWPM_CALLOUT_FLAG_REGISTERED = 0x00040000;

        public const uint FWPM_SESSION_FLAG_DYNAMIC = 0x00000001;
        public const uint INFINITE = 0xffffffff;
        public const uint FWPM_GENERAL_CONTEXT = 8;
        public const int FWP_V6_ADDR_SIZE = 16;
        public const int FWP_OPTION_VALUE_ALLOW_MULTICAST_STATE = 0;
        public const int FWP_OPTION_VALUE_DENY_MULTICAST_STATE = 1;
        public const int FWP_OPTION_VALUE_ALLOW_GLOBAL_MULTICAST_STATE = 2;
        public const int FWP_OPTION_VALUE_DISABLE_LOOSE_SOURCE = 0;
        public const int FWP_OPTION_VALUE_ENABLE_LOOSE_SOURCE = 1;
        public const int FWP_ACTION_FLAG_TERMINATING = 4096;
        public const int FWP_ACTION_FLAG_NON_TERMINATING = 8192;
        public const int FWP_ACTION_FLAG_CALLOUT = 16384;
        public const int FWPM_FILTER_FLAG_NONE = 0;
        public const int FWPM_FILTER_FLAG_PERSISTENT = 1;
        public const int FWPM_FILTER_FLAG_BOOTTIME = 2;
        public const int FWPM_FILTER_FLAG_HAS_PROVIDER_CONTEXT = 4;
        public const int FWPM_FILTER_FLAG_CLEAR_ACTION_RIGHT = 8;
        public const int FWPM_FILTER_FLAG_PERMIT_IF_CALLOUT_UNREGISTERED = 16;
        public const int FWPM_FILTER_FLAG_DISABLED = 32;
        public const int FWPM_FILTER_FLAG_INDEXED = 64;
        public const int FWPM_FILTER_FLAG_HAS_SECURITY_REALM_PROVIDER_CONTEXT = 128;
        public const int FWPM_FILTER_FLAG_SYSTEMOS_ONLY = 256;
        public const int FWPM_FILTER_FLAG_GAMEOS_ONLY = 512;
        public const uint RPC_C_AUTHN_WINNT = 10;
        public const uint RPC_C_AUTHN_DEFAULT = 0xffffffff;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SID_IDENTIFIER_AUTHORITY
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
        public byte[] Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SID
    {
        public byte Revision;
        public byte SubAuthorityCount;
        public SID_IDENTIFIER_AUTHORITY IdentifierAuthority;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.U4)]
        public uint[] SubAuthority;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SEC_WINNT_AUTH_IDENTITY_W
    {
        public IntPtr User;
        public uint UserLength;
        public IntPtr Domain;
        public uint DomainLength;
        public IntPtr Password;
        public uint PasswordLength;
        public uint Flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWP_BYTE_BLOB_
    {
        public uint size;
        public IntPtr data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWPM_DISPLAY_DATA0_
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string name;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string description;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWPM_SESSION0
    {
        public Guid sessionKey;
        public FWPM_DISPLAY_DATA0_ displayData;
        public uint flags;
        public uint txnWaitTimeoutInMSec;
        public uint processId;
        public IntPtr sid;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string username;

        public int kernelMode;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FWPM_PROVIDER_CONTEXT0_Union
    {
        [FieldOffset(0)]
        public IntPtr keyingPolicy;

        [FieldOffset(0)]
        public IntPtr ikeQmTransportPolicy;

        [FieldOffset(0)]
        public IntPtr ikeQmTunnelPolicy;

        [FieldOffset(0)]
        public IntPtr authipQmTransportPolicy;

        [FieldOffset(0)]
        public IntPtr authipQmTunnelPolicy;

        [FieldOffset(0)]
        public IntPtr ikeMmPolicy;

        [FieldOffset(0)]
        public IntPtr authIpMmPolicy;

        [FieldOffset(0)]
        public IntPtr dataBuffer;

        [FieldOffset(0)]
        public IntPtr classifyOptions;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWPM_PROVIDER_CONTEXT0
    {
        public Guid providerContextKey;
        public FWPM_DISPLAY_DATA0_ displayData;
        public uint flags;
        public IntPtr providerKey;
        public FWP_BYTE_BLOB_ providerData;
        public FWPM_PROVIDER_CONTEXT_TYPE type;
        public FWPM_PROVIDER_CONTEXT0_Union union;

        public ulong providerContextId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWP_V4_ADDR_AND_MASK
    {
        public uint addr;
        public uint mask;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FWP_CONDITION_VALUE0_Union
    {
        [FieldOffset(0)]
        public byte uint8;

        [FieldOffset(0)]
        public ushort uint16;

        [FieldOffset(0)]
        public uint uint32;

        [FieldOffset(0)]
        public IntPtr uint64;

        [FieldOffset(0)]
        public byte int8;

        [FieldOffset(0)]
        public short int16;

        [FieldOffset(0)]
        public int int32;

        [FieldOffset(0)]
        public IntPtr int64;

        [FieldOffset(0)]
        public float float32;

        [FieldOffset(0)]
        public IntPtr double64;

        [FieldOffset(0)]
        public IntPtr byteArray16;

        [FieldOffset(0)]
        public IntPtr byteBlob;

        [FieldOffset(0)]
        public IntPtr sid;

        [FieldOffset(0)]
        public IntPtr sd;

        [FieldOffset(0)]
        public IntPtr tokenInformation;

        [FieldOffset(0)]
        public IntPtr tokenAccessInformation;

        [FieldOffset(0)]
        public IntPtr unicodeString;

        [FieldOffset(0)]
        public IntPtr byteArray6;

        [FieldOffset(0)]
        public IntPtr v4AddrMask;

        [FieldOffset(0)]
        public IntPtr v6AddrMask;

        [FieldOffset(0)]
        public IntPtr rangeValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWP_CONDITION_VALUE0_
    {
        public FWP_DATA_TYPE_ type;
        public FWP_CONDITION_VALUE0_Union Union1;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWPM_FILTER_CONDITION0_
    {
        public Guid fieldKey;
        public FWP_MATCH_TYPE_ matchType;
        public FWP_CONDITION_VALUE0_ conditionValue;
    }

    [StructLayout(LayoutKind.Sequential)] 
    public struct FWPM_FILTER0_
    {
        public Guid filterKey;
        public FWPM_DISPLAY_DATA0_ displayData;
        public uint flags;
        public IntPtr providerKey;
        public FWP_BYTE_BLOB_ providerData;
        public Guid layerKey;
        public Guid subLayerKey;
        public FWP_VALUE0_ weight;
        public uint numFilterConditions;
        public IntPtr filterCondition;
        public FWPM_ACTION0_ action;
        public int offset; // I do not know why. SO IT WAS IN GO CODE AND SO WORKS. BUT NOT SO IN THE DOCUMENTATION
        public Guid providerContextKey;
        public IntPtr reserved;
        public ulong filterId;
        public FWP_VALUE0_ effectiveWeight;
    }

    public enum FWP_ACTION_TYPE
    {
        FWP_ACTION_BLOCK = 1 | 4096,
        FWP_ACTION_PERMIT = 2 | 4096,
        FWP_ACTION_CALLOUT_TERMINATING = 3 | 16384 | 4096,
        FWP_ACTION_CALLOUT_INSPECTION = 4 | 16384 | 4096,
        FWP_ACTION_CALLOUT_UNKNOWN = 5 | 16384,
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FWPM_ACTION0_Union
    {
        [FieldOffset(0)]
        public Guid filterType;

        [FieldOffset(0)]
        public Guid calloutKey;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWPM_ACTION0_
    {
        public FWP_ACTION_TYPE type;
        public FWPM_ACTION0_Union Union1;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWP_VALUE0_
    {
        public FWP_DATA_TYPE_ type;
        public FWP_VALUE0_Union Union1;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FWP_VALUE0_Union
    {
        [FieldOffset(0)]
        public byte uint8;

        [FieldOffset(0)]
        public ushort uint16;

        [FieldOffset(0)]
        public uint uint32;

        [FieldOffset(0)]
        public IntPtr uint64;

        [FieldOffset(0)]
        public byte int8;

        [FieldOffset(0)]
        public short int16;

        [FieldOffset(0)]
        public int int32;

        [FieldOffset(0)]
        public IntPtr int64;

        [FieldOffset(0)]
        public float float32;

        [FieldOffset(0)]
        public IntPtr double64;

        [FieldOffset(0)]
        public IntPtr byteArray16;

        [FieldOffset(0)]
        public IntPtr byteBlob;

        [FieldOffset(0)]
        public IntPtr sid;

        [FieldOffset(0)]
        public IntPtr sd;

        [FieldOffset(0)]
        public IntPtr tokenInformation;

        [FieldOffset(0)]
        public IntPtr tokenAccessInformation;

        [FieldOffset(0)]
        public IntPtr unicodeString;

        [FieldOffset(0)]
        public IntPtr byteArray6;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWPM_PROVIDER0
    {
        public Guid providerKey;
        public FWPM_DISPLAY_DATA0_ displayData;
        public uint flags;
        public FWP_BYTE_BLOB_ providerData;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string serviceName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FWPM_CALLOUT0
    {
        public Guid calloutKey;
        public FWPM_DISPLAY_DATA0_ displayData;
        public uint flags;
        public IntPtr providerKey;
        public FWP_BYTE_BLOB_ providerData;
        public Guid applicableLayer;
        public uint calloutId;
    }

    public enum FWPM_PROVIDER_CONTEXT_TYPE
    {
        FWPM_IPSEC_KEYING_CONTEXT,
        FWPM_IPSEC_IKE_QM_TRANSPORT_CONTEXT,
        FWPM_IPSEC_IKE_QM_TUNNEL_CONTEXT,
        FWPM_IPSEC_AUTHIP_QM_TRANSPORT_CONTEXT,
        FWPM_IPSEC_AUTHIP_QM_TUNNEL_CONTEXT,
        FWPM_IPSEC_IKE_MM_CONTEXT,
        FWPM_IPSEC_AUTHIP_MM_CONTEXT,
        FWPM_CLASSIFY_OPTIONS_CONTEXT,
        FWPM_GENERAL_CONTEXT,
        FWPM_IPSEC_IKEV2_QM_TUNNEL_CONTEXT,
        FWPM_IPSEC_IKEV2_MM_CONTEXT,
        FWPM_IPSEC_DOSP_CONTEXT,
        FWPM_IPSEC_IKEV2_QM_TRANSPORT_CONTEXT,
        FWPM_PROVIDER_CONTEXT_TYPE_MAX
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct FWPM_SUBLAYER0_
    {
        public Guid subLayerKey;
        public FWPM_DISPLAY_DATA0_ displayData;
        public ushort flags;
        public IntPtr providerKey;
        public FWP_BYTE_BLOB_ providerData;
        public ushort weight;
    }

    public enum WtIPProto
    {
        cIPPROTO_ICMP = 1,
        cIPPROTO_ICMPV6 = 58,
        cIPPROTO_IPinIP = 4,
        cIPPROTO_TCP = 6,
        cIPPROTO_UDP = 17,
        cIPPROTO_ESP = 50,
        cIPPROTO_AH = 51,
    };

    public enum FWP_DATA_TYPE_
    {
        FWP_EMPTY = 0,
        FWP_UINT8 = (FWP_EMPTY + 1),
        FWP_UINT16 = (FWP_UINT8 + 1),
        FWP_UINT32 = (FWP_UINT16 + 1),
        FWP_UINT64 = (FWP_UINT32 + 1),
        FWP_INT8 = (FWP_UINT64 + 1),
        FWP_INT16 = (FWP_INT8 + 1),
        FWP_INT32 = (FWP_INT16 + 1),
        FWP_INT64 = (FWP_INT32 + 1),
        FWP_FLOAT = (FWP_INT64 + 1),
        FWP_DOUBLE = (FWP_FLOAT + 1),
        FWP_BYTE_ARRAY16_TYPE = (FWP_DOUBLE + 1),
        FWP_BYTE_BLOB_TYPE = (FWP_BYTE_ARRAY16_TYPE + 1),
        FWP_SID = (FWP_BYTE_BLOB_TYPE + 1),
        FWP_SECURITY_DESCRIPTOR_TYPE = (FWP_SID + 1),
        FWP_TOKEN_INFORMATION_TYPE = (FWP_SECURITY_DESCRIPTOR_TYPE + 1),
        FWP_TOKEN_ACCESS_INFORMATION_TYPE = (FWP_TOKEN_INFORMATION_TYPE + 1),
        FWP_UNICODE_STRING_TYPE = (FWP_TOKEN_ACCESS_INFORMATION_TYPE + 1),
        FWP_BYTE_ARRAY6_TYPE = (FWP_UNICODE_STRING_TYPE + 1),
        FWP_BITMAP_INDEX_TYPE = (FWP_BYTE_ARRAY6_TYPE + 1),
        FWP_BITMAP_ARRAY64_TYPE = (FWP_BITMAP_INDEX_TYPE + 1),
        FWP_SINGLE_DATA_TYPE_MAX = 0xff,
        FWP_V4_ADDR_MASK = (FWP_SINGLE_DATA_TYPE_MAX + 1),
        FWP_V6_ADDR_MASK = (FWP_V4_ADDR_MASK + 1),
        FWP_RANGE_TYPE = (FWP_V6_ADDR_MASK + 1),
        FWP_DATA_TYPE_MAX = (FWP_RANGE_TYPE + 1)
    };

    public enum FWP_MATCH_TYPE_
    {
        FWP_MATCH_EQUAL = 0,
        FWP_MATCH_GREATER = (FWP_MATCH_EQUAL + 1),
        FWP_MATCH_LESS = (FWP_MATCH_GREATER + 1),
        FWP_MATCH_GREATER_OR_EQUAL = (FWP_MATCH_LESS + 1),
        FWP_MATCH_LESS_OR_EQUAL = (FWP_MATCH_GREATER_OR_EQUAL + 1),
        FWP_MATCH_RANGE = (FWP_MATCH_LESS_OR_EQUAL + 1),
        FWP_MATCH_FLAGS_ALL_SET = (FWP_MATCH_RANGE + 1),
        FWP_MATCH_FLAGS_ANY_SET = (FWP_MATCH_FLAGS_ALL_SET + 1),
        FWP_MATCH_FLAGS_NONE_SET = (FWP_MATCH_FLAGS_ANY_SET + 1),
        FWP_MATCH_EQUAL_CASE_INSENSITIVE = (FWP_MATCH_FLAGS_NONE_SET + 1),
        FWP_MATCH_NOT_EQUAL = (FWP_MATCH_EQUAL_CASE_INSENSITIVE + 1),
        FWP_MATCH_PREFIX = (FWP_MATCH_NOT_EQUAL + 1),
        FWP_MATCH_NOT_PREFIX = (FWP_MATCH_PREFIX + 1),
        FWP_MATCH_TYPE_MAX = (FWP_MATCH_NOT_PREFIX + 1)
    };
}