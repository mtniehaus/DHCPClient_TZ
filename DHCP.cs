using System;
using System.Runtime.InteropServices;

namespace DHCPClient_TZ
{
    class DHCP
    {
        private struct DHCPCAPI_CLASSID
        {
            public UInt32 Flags;
            public IntPtr Data;
            public UInt32 nBytesData;
        }

        private struct DHCPCAPI_PARAMS
        {
            public UInt32 Flags;
            public UInt32 OptionId;
            [MarshalAs(UnmanagedType.Bool)]
            public bool IsVendor;
            public IntPtr Data;
            public UInt32 nBytesData;
        }

        private struct DHCPCAPI_PARAMS_ARRAY
        {
            public UInt32 nParams;
            public IntPtr Params;
        }

        [DllImport("Dhcpcsvc.dll", CharSet = CharSet.Unicode)]
        private static extern uint DhcpRequestParams(uint Flags, IntPtr Reserved,
         string AdapterName, IntPtr ClassId, DHCPCAPI_PARAMS_ARRAY SendParams,
         DHCPCAPI_PARAMS_ARRAY RecdParams, IntPtr Buffer, ref uint pSize,
         string RequestIdStr);

         // private static extern uint DhcpRequestParams(uint Flags, IntPtr Reserved,
         // string AdapterName, ref DHCPCAPI_CLASSID ClassId, DHCPCAPI_PARAMS_ARRAY SendParams,
         // DHCPCAPI_PARAMS_ARRAY RecdParams, IntPtr Buffer, ref uint pSize,
         // string RequestIdStr);

        private static IntPtr StructToPtr(object obj)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }

        public static void GetOption(string adapterName, UInt32 optionId)
        {
            uint flags = 2; // DHCPCAPI_REQUEST_SYNCHRONOUS

            DHCPCAPI_CLASSID classId = new DHCPCAPI_CLASSID();

            DHCPCAPI_PARAMS_ARRAY sendParams = new DHCPCAPI_PARAMS_ARRAY();
            sendParams.nParams = 0;
            sendParams.Params = IntPtr.Zero;

            DHCPCAPI_PARAMS request = new DHCPCAPI_PARAMS();
            request.Flags = 0;
            request.IsVendor = false;
            request.OptionId = optionId;
            request.Data = IntPtr.Zero;
            request.nBytesData = 0;

            DHCPCAPI_PARAMS_ARRAY requestParams = new DHCPCAPI_PARAMS_ARRAY();
            requestParams.nParams = 1;
            requestParams.Params = StructToPtr(request);

            IntPtr bufPtr = Marshal.AllocCoTaskMem(1000);
            uint size = 1000;
            uint result = DhcpRequestParams(flags, IntPtr.Zero, adapterName, IntPtr.Zero, sendParams, requestParams, bufPtr, ref size, null);
        }

    }
}
