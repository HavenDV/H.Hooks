using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace WeVPN.Firewall
{
    public static class WfpMethods
    {
        public static IntPtr CreateWfpSession(string name, string description)
        {
            var session = new FWPM_SESSION0
            {
                displayData = new FWPM_DISPLAY_DATA0_
                {
                    name = name,
                    description = description,
                },
                flags = NativeConstants.FWPM_SESSION_FLAG_DYNAMIC,
                txnWaitTimeoutInMSec = NativeConstants.INFINITE,
            };

            var result = NativeMethods.FwpmEngineOpen0(
                null,
                NativeConstants.RPC_C_AUTHN_WINNT,
                IntPtr.Zero,
                ref session,
                out var ptr);
            EnsureResultIsNull(result);

            return ptr;
        }

        public static void CloseWfpSession(IntPtr ptr)
        {
            var result = NativeMethods.FwpmEngineClose0(ptr);
            EnsureResultIsNull(result);
        }

        public static void BeginTransaction(IntPtr ptr)
        {
            var result = NativeMethods.FwpmTransactionBegin0(ptr, 0);
            EnsureResultIsNull(result);
        }

        public static void CommitTransaction(IntPtr ptr)
        {
            var result = NativeMethods.FwpmTransactionCommit0(ptr);
            EnsureResultIsNull(result);
        }

        public static void AbortTransaction(IntPtr ptr)
        {
            var result = NativeMethods.FwpmTransactionAbort0(ptr);
            EnsureResultIsNull(result);
        }

        public static Guid AddProviderContext(IntPtr ptr, Guid providerKey, string name, string description, IPAddress ipAddress)
        {
            using (var providerKeyPtr = new StructurePtr<Guid>(providerKey))
            using (var addressPtr = new BytesPtr(ipAddress.GetAddressBytes()))
            using (var blobPtr = new StructurePtr<FWP_BYTE_BLOB_>(new FWP_BYTE_BLOB_
            {
                data = addressPtr,
                size = 4,
            }))
            {
                var id = 0UL;
                var guid = Guid.NewGuid();
                var context = new FWPM_PROVIDER_CONTEXT0
                {
                    displayData = new FWPM_DISPLAY_DATA0_
                    {
                        name = name,
                        description = description,
                    },
                    type = FWPM_PROVIDER_CONTEXT_TYPE.FWPM_GENERAL_CONTEXT,
                    providerContextKey = guid,
                    union = new FWPM_PROVIDER_CONTEXT0_Union
                    {
                        dataBuffer = blobPtr,
                    },
                    providerKey = providerKeyPtr,
                };
                var result = NativeMethods.FwpmProviderContextAdd0(ptr, ref context, IntPtr.Zero, ref id);

                EnsureResultIsNull(result);

                return guid;
            }
        }

        public static Guid AddProvider(IntPtr ptr, string name, string description)
        {
            var guid = Guid.NewGuid();
            var provider = new FWPM_PROVIDER0
            {
                providerKey = guid,
                displayData = new FWPM_DISPLAY_DATA0_
                {
                    name = name,
                    description = description,
                },
            };
            var result = NativeMethods.FwpmProviderAdd0(ptr, ref provider, IntPtr.Zero);

            EnsureResultIsNull(result);

            return guid;
        }

        public static Guid AddSubLayer(IntPtr ptr, Guid providerKey, string name, string description)
        {
            using (var providerKeyPtr = new StructurePtr<Guid>(providerKey))
            {
                var guid = Guid.NewGuid();
                var provider = new FWPM_SUBLAYER0_
                {
                    subLayerKey = guid,
                    displayData = new FWPM_DISPLAY_DATA0_
                    {
                        name = name,
                        description = description,
                    },
                    providerKey = providerKeyPtr,
                    flags = 0,
                    weight = 0,
                };
                var result = NativeMethods.FwpmSubLayerAdd0(ptr, ref provider, IntPtr.Zero);

                EnsureResultIsNull(result);

                return guid;
            }
        }

        public static IntPtrWrapper GetAppIdFromFileName(string fileName)
        {
            var result = NativeMethods.FwpmGetAppIdFromFileName0(fileName, out var ptr);
            EnsureResultIsNull(result);

            return new IntPtrWrapper(ptr, FreeMemory);
        }

        public static void FreeMemory(IntPtr ptr)
        {
            NativeMethods.FwpmFreeMemory0(ptr);
        }

        public static Guid AddCallout(
            IntPtr ptr, Guid calloutKey, Guid providerKey, Guid applicableLayer, string name, string description)
        {
            using (var providerKeyPtr = new StructurePtr<Guid>(providerKey))
            {
                var id = 0U;
                var callout = new FWPM_CALLOUT0
                {
                    calloutKey = calloutKey,
                    providerKey = providerKeyPtr,
                    displayData = new FWPM_DISPLAY_DATA0_
                    {
                        name = name,
                        description = description,
                    },
                    applicableLayer = applicableLayer,
                    flags = NativeConstants.cFWPM_CALLOUT_FLAG_USES_PROVIDER_CONTEXT,
                };
                var result = NativeMethods.FwpmCalloutAdd0(ptr, ref callout, IntPtr.Zero, ref id);

                EnsureResultIsNull(result);

                return calloutKey;
            }
        }

        public static Guid AllowSplitAppIds(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            IntPtrWrapper[] appIds,
            byte weight,
            Guid providerContextKey,
            Guid actionFilterGuid,
            bool reversed,
            string name,
            string description)
        {
            using (var providerKeyPtr = new StructurePtr<Guid>(providerKey))
            using (var conditionsPtr = new ArrayPtr<FWPM_FILTER_CONDITION0_>(appIds
                .Select(appId => new FWPM_FILTER_CONDITION0_
                {
                    fieldKey = NativeConstants.cFWPM_CONDITION_ALE_APP_ID,
                    matchType = reversed
                        ? FWP_MATCH_TYPE_.FWP_MATCH_NOT_EQUAL
                        : FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                    conditionValue = new FWP_CONDITION_VALUE0_
                    {
                        type = FWP_DATA_TYPE_.FWP_BYTE_BLOB_TYPE,
                        Union1 = new FWP_CONDITION_VALUE0_Union
                        {
                            byteBlob = appId.IntPtr,
                        }
                    }
                })
                .ToArray()))
            {
                var id = 0UL;
                var guid = Guid.NewGuid();
                var filter = new FWPM_FILTER0_
                {
                    filterKey = guid,
                    providerKey = providerKeyPtr,
                    subLayerKey = subLayerKey,
                    weight = new FWP_VALUE0_
                    {
                        type = FWP_DATA_TYPE_.FWP_UINT8,
                        Union1 = new FWP_VALUE0_Union
                        {
                            uint8 = weight,
                        }
                    },
                    numFilterConditions = (uint)appIds.Length,
                    filterCondition = conditionsPtr,
                    flags = NativeConstants.FWPM_FILTER_FLAG_HAS_PROVIDER_CONTEXT,
                    action = new FWPM_ACTION0_
                    {
                        type = FWP_ACTION_TYPE.FWP_ACTION_CALLOUT_UNKNOWN,
                        Union1 = new FWPM_ACTION0_Union
                        {
                            filterType = actionFilterGuid,
                        }
                    },
                    displayData = new FWPM_DISPLAY_DATA0_
                    {
                        name = name,
                        description = description,
                    },
                    providerContextKey = providerContextKey,
                    layerKey = layerKey,
                };
                var result = NativeMethods.FwpmFilterAdd0(engineHandle, ref filter, IntPtr.Zero, ref id);

                EnsureResultIsNull(result);

                return guid;
            }
        }

        public static Guid PermitAppId(
            IntPtr engineHandle, 
            Guid providerKey, 
            Guid subLayerKey, 
            Guid layerKey,
            IntPtrWrapper appId, 
            byte weight, 
            string name, 
            string description)
        {
            return AddFilter(engineHandle, providerKey, subLayerKey, layerKey, weight, name, description,
                FWP_ACTION_TYPE.FWP_ACTION_PERMIT,
                new[]{
                    new FWPM_FILTER_CONDITION0_
                    {
                        fieldKey = NativeConstants.cFWPM_CONDITION_ALE_APP_ID,
                        matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                        conditionValue = new FWP_CONDITION_VALUE0_
                        {
                            type = FWP_DATA_TYPE_.FWP_BYTE_BLOB_TYPE,
                            Union1 = new FWP_CONDITION_VALUE0_Union
                            {
                                byteBlob = appId.IntPtr,
                            }
                        }
                    },
                });
        }

        public static Guid PermitLoopback(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            byte weight,
            string name,
            string description)
        {
            return AddFilter(engineHandle, providerKey, subLayerKey, layerKey, weight, name, description,
                FWP_ACTION_TYPE.FWP_ACTION_PERMIT,
                new[]{
                    new FWPM_FILTER_CONDITION0_
                    {
                        fieldKey = NativeConstants.FWPM_CONDITION_FLAGS,
                        matchType = FWP_MATCH_TYPE_.FWP_MATCH_FLAGS_ALL_SET,
                        conditionValue = new FWP_CONDITION_VALUE0_
                        {
                            type = FWP_DATA_TYPE_.FWP_UINT32,
                            Union1 = new FWP_CONDITION_VALUE0_Union
                            {
                                uint32 = NativeConstants.cFWP_CONDITION_FLAG_IS_LOOPBACK,
                            }
                        }
                    },
                });
        }

        public static Guid BlockAll(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            byte weight,
            string name,
            string description)
        {
            return AddFilter(engineHandle, providerKey, subLayerKey, layerKey, weight, name, description,
                FWP_ACTION_TYPE.FWP_ACTION_BLOCK);
        }

        public static FWPM_FILTER_CONDITION0_[] DnsConditions { get; } = {
            new FWPM_FILTER_CONDITION0_
            {
                fieldKey = NativeConstants.cFWPM_CONDITION_IP_REMOTE_PORT,
                matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                conditionValue = new FWP_CONDITION_VALUE0_
                {
                    type = FWP_DATA_TYPE_.FWP_UINT16,
                    Union1 = new FWP_CONDITION_VALUE0_Union
                    {
                        uint16 = 53, // DNS PORT
                    }
                }
            },
            new FWPM_FILTER_CONDITION0_
            {
                fieldKey = NativeConstants.cFWPM_CONDITION_IP_PROTOCOL,
                matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                conditionValue = new FWP_CONDITION_VALUE0_
                {
                    type = FWP_DATA_TYPE_.FWP_UINT8,
                    Union1 = new FWP_CONDITION_VALUE0_Union
                    {
                        uint8 = (byte)WtIPProto.cIPPROTO_UDP,
                    }
                }
            },
            new FWPM_FILTER_CONDITION0_
            {
                fieldKey = NativeConstants.cFWPM_CONDITION_IP_PROTOCOL,
                matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                conditionValue = new FWP_CONDITION_VALUE0_
                {
                    type = FWP_DATA_TYPE_.FWP_UINT8,
                    Union1 = new FWP_CONDITION_VALUE0_Union
                    {
                        uint8 = (byte)WtIPProto.cIPPROTO_TCP,
                    }
                }
            },
        };

        public static Guid BlockDns(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            byte weight,
            string name,
            string description)
        {
            return AddFilter(engineHandle, providerKey, subLayerKey, layerKey, weight, name, description,
                FWP_ACTION_TYPE.FWP_ACTION_BLOCK, DnsConditions);
        }

        public static Guid AllowDnsV6(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            byte weight,
            IEnumerable<IPAddress> addresses,
            string name,
            string description)
        {
            var ptrs = addresses
                .Select(address => new BytesPtr(address.GetAddressBytes()))
                .ToArray();

            try
            {
                return AddFilter(engineHandle, providerKey, subLayerKey, layerKey, weight, name, description,
                    FWP_ACTION_TYPE.FWP_ACTION_PERMIT,
                    DnsConditions
                        .Concat(ptrs.Select(ptr => new FWPM_FILTER_CONDITION0_
                        {
                            fieldKey = NativeConstants.cFWPM_CONDITION_IP_REMOTE_ADDRESS,
                            matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                            conditionValue = new FWP_CONDITION_VALUE0_
                            {
                                type = FWP_DATA_TYPE_.FWP_BYTE_ARRAY16_TYPE,
                                Union1 = new FWP_CONDITION_VALUE0_Union
                                {
                                    byteArray16 = ptr,
                                }
                            }
                        }))
                        .ToArray());
            }
            finally
            {
                foreach (var ptr in ptrs)
                {
                    ptr.Dispose();
                }
            }
        }

        public static Guid PermitNetworkInterface(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            byte weight,
            ulong ifLuid,
            string name,
            string description)
        {
            using (var ifLuidPtr = new StructurePtr<ulong>(ifLuid))
            {
                return AddFilter(engineHandle, providerKey, subLayerKey, layerKey, weight, name, description,
                    FWP_ACTION_TYPE.FWP_ACTION_PERMIT,
                    new[]
                    {
                        new FWPM_FILTER_CONDITION0_
                        {
                            fieldKey = NativeConstants.cFWPM_CONDITION_IP_LOCAL_INTERFACE,
                            matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                            conditionValue = new FWP_CONDITION_VALUE0_
                            {
                                type = FWP_DATA_TYPE_.FWP_UINT64,
                                Union1 = new FWP_CONDITION_VALUE0_Union
                                {
                                    uint64 = ifLuidPtr,
                                }
                            }
                        },
                    });
            }
        }

        public static Guid PermitUdpPortV4(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            byte weight,
            ushort port,
            string name,
            string description)
        {
            return AddFilter(engineHandle, providerKey, subLayerKey, layerKey, weight, name, description,
                FWP_ACTION_TYPE.FWP_ACTION_PERMIT,
                new[]{
                    new FWPM_FILTER_CONDITION0_
                    {
                        fieldKey = NativeConstants.cFWPM_CONDITION_IP_PROTOCOL,
                        matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                        conditionValue = new FWP_CONDITION_VALUE0_
                        {
                            type = FWP_DATA_TYPE_.FWP_UINT8,
                            Union1 = new FWP_CONDITION_VALUE0_Union
                            {
                                uint8 = (byte)WtIPProto.cIPPROTO_UDP,
                            }
                        }
                    },
                    new FWPM_FILTER_CONDITION0_
                    {
                        fieldKey = NativeConstants.cFWPM_CONDITION_IP_REMOTE_PORT,
                        matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                        conditionValue = new FWP_CONDITION_VALUE0_
                        {
                            type = FWP_DATA_TYPE_.FWP_UINT16,
                            Union1 = new FWP_CONDITION_VALUE0_Union
                            {
                                uint16 = port,
                            }
                        }
                    },
                });
        }

        public static Guid PermitProtocolV4(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            byte weight,
            byte proto,
            string name,
            string description)
        {
            return AddFilter(engineHandle, providerKey, subLayerKey, layerKey, weight, name, description, 
                FWP_ACTION_TYPE.FWP_ACTION_PERMIT,
                new[]{
                    new FWPM_FILTER_CONDITION0_
                    {
                        fieldKey = NativeConstants.cFWPM_CONDITION_IP_PROTOCOL,
                        matchType = FWP_MATCH_TYPE_.FWP_MATCH_EQUAL,
                        conditionValue = new FWP_CONDITION_VALUE0_
                        {
                            type = FWP_DATA_TYPE_.FWP_UINT8,
                            Union1 = new FWP_CONDITION_VALUE0_Union
                            {
                                uint8 = proto,
                            }
                        }
                    },
                });
        }

        public static Guid AddFilter(
            IntPtr engineHandle,
            Guid providerKey,
            Guid subLayerKey,
            Guid layerKey,
            byte weight,
            string name,
            string description,
            FWP_ACTION_TYPE actionType,
            FWPM_FILTER_CONDITION0_[]? conditions = null)
        {
            using (var providerKeyPtr = new StructurePtr<Guid>(providerKey))
            using (var conditionsPtr = new ArrayPtr<FWPM_FILTER_CONDITION0_>(conditions))
            {
                var id = 0UL;
                var guid = Guid.NewGuid();
                var filter = new FWPM_FILTER0_
                {
                    filterKey = guid,
                    providerKey = providerKeyPtr,
                    subLayerKey = subLayerKey,
                    weight = new FWP_VALUE0_
                    {
                        type = FWP_DATA_TYPE_.FWP_UINT8,
                        Union1 = new FWP_VALUE0_Union
                        {
                            uint8 = weight,
                        }
                    },
                    numFilterConditions = (uint)(conditions?.Length ?? 0),
                    filterCondition = conditionsPtr,
                    action = new FWPM_ACTION0_
                    {
                        type = actionType,
                    },
                    displayData = new FWPM_DISPLAY_DATA0_
                    {
                        name = name,
                        description = description,
                    },
                    layerKey = layerKey,
                };
                var result = NativeMethods.FwpmFilterAdd0(engineHandle, ref filter, IntPtr.Zero, ref id);

                EnsureResultIsNull(result);

                return guid;
            }
        }

        public static void EnsureResultIsNull(uint result)
        {
            Marshal.ThrowExceptionForHR((int)result);
        }
    }
}
