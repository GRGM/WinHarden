using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net;

namespace InformationUtils
{

    internal class TerminalSessionData
    {
        public int SessionId;
        public TerminalSessionUtils.WTS_CONNECTSTATE_CLASS ConnectionState;
        public string StationName;

        public TerminalSessionData(int sessionId, TerminalSessionUtils.WTS_CONNECTSTATE_CLASS connState, string stationName)
        {
            SessionId = sessionId;
            ConnectionState = connState;
            StationName = stationName;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", SessionId, ConnectionState, StationName);
        }
    }

    internal class TerminalSessionInfo
    {
        public string InitialProgram;
        public string ApplicationName;
        public string WorkingDirectory;
        public string OEMId;
        public int SessionId;
        public string UserName;
        public string WinStationName;
        public string DomainName;
        public TerminalSessionUtils.WTS_CONNECTSTATE_CLASS ConnectState;
        public int ClientBuildNumber;
        public string ClientName;
        public string ClientDirectory;
        public int ClientProductId;
        public int ClientHardwareId;
        public TerminalSessionUtils.WTS_CLIENT_ADDRESS ClientAddress;
        public TerminalSessionUtils.WTS_CLIENT_DISPLAY ClientDisplay;
        public int ClientProtocolType;
    }

    internal static class TerminalSessionUtils
    {

        [DllImport("wtsapi32.dll")]
        static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

        [DllImport("wtsapi32.dll")]
        static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass,
            out System.IntPtr ppBuffer, out uint pBytesReturned);

        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSEnumerateSessions(IntPtr hServer, [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version, ref IntPtr ppSessionInfo, [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pMemory);

        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public Int32 SessionID;
            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;
            public WTS_CONNECTSTATE_CLASS State;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WTS_CLIENT_ADDRESS
        {
            public uint AddressFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] Address;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WTS_CLIENT_DISPLAY
        {
            public uint HorizontalResolution;
            public uint VerticalResolution;
            public uint ColorDepth;
        }

        internal enum WTS_CONNECTSTATE_CLASS
        {
            Active,
            Connected,
            ConnectQuery,
            Shadow,
            Disconnected,
            Idle,
            Listen,
            Reset,
            Down,
            Init
        }

        internal enum WTS_INFO_CLASS
        {
            InitialProgram = 0,
            ApplicationName = 1,
            WorkingDirectory = 2,
            OEMId = 3,
            SessionId = 4,
            UserName = 5,
            WinStationName = 6,
            DomainName = 7,
            ConnectState = 8,
            ClientBuildNumber = 9,
            ClientName = 10,
            ClientDirectory = 11,
            ClientProductId = 12,
            ClientHardwareId = 13,
            ClientAddress = 14,
            ClientDisplay = 15,
            ClientProtocolType = 16
        }

        private static IntPtr OpenServer(string Name)
        {
            IntPtr server = WTSOpenServer(Name);
            return server;
        }

        private static void CloseServer(IntPtr ServerHandle)
        {
            WTSCloseServer(ServerHandle);
        }


        internal static string GetTerminalServicesSessions(string server)
        {
            string result = "";
            List<TerminalSessionData> terms = ListSessions(server);
            foreach (TerminalSessionData term in terms)
            {
                TerminalSessionInfo termInfo = GetSessionInfo(server, term.SessionId);
                if(termInfo.UserName is null)
                {
                    continue;
                }
                result += TerminalSessionInfoToString(termInfo) + "\r\n";
            }
            return result;
        }
        private static List<TerminalSessionData> ListSessions(string ServerName)
        {
            IntPtr server = IntPtr.Zero;
            List<TerminalSessionData> ret = new List<TerminalSessionData>();
            server = OpenServer(ServerName);

            try
            {
                IntPtr ppSessionInfo = IntPtr.Zero;

                Int32 count = 0;
                Int32 retval = WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count);
                Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));

                var current = (Int64)0;
                if (Environment.Is64BitProcess)
                {
                    current = (long)ppSessionInfo;
                }
                else
                {
                    current = (int)ppSessionInfo;
                }

                if (retval != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));
                        current += dataSize;

                        ret.Add(new TerminalSessionData(si.SessionID, si.State, si.pWinStationName));
                    }

                    WTSFreeMemory(ppSessionInfo);
                }
            }
            finally
            {
                CloseServer(server);
            }

            return ret;
        }

        private static TerminalSessionInfo GetSessionInfo(string ServerName, int SessionId)
        {
            IntPtr server = IntPtr.Zero;
            server = OpenServer(ServerName);
            System.IntPtr buffer = IntPtr.Zero;
            uint bytesReturned;
            TerminalSessionInfo data = new TerminalSessionInfo();

            try
            {
                bool worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ApplicationName, out buffer, out bytesReturned);

                if (!worked)
                    return data;

                string strData = Marshal.PtrToStringAnsi(buffer);
                data.ApplicationName = strData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ClientAddress, out buffer, out bytesReturned);

                if (!worked)
                    return data;

                WTS_CLIENT_ADDRESS si = (WTS_CLIENT_ADDRESS)Marshal.PtrToStructure((System.IntPtr)buffer, typeof(WTS_CLIENT_ADDRESS));
                data.ClientAddress = si;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ClientBuildNumber, out buffer, out bytesReturned);

                if (!worked)
                    return data;

                int lData = Marshal.ReadInt32(buffer);
                data.ClientBuildNumber = lData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ClientDirectory, out buffer, out bytesReturned);

                if (!worked)
                    return data;

                strData = Marshal.PtrToStringAnsi(buffer);
                data.ClientDirectory = strData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ClientDisplay, out buffer, out bytesReturned);

                if (!worked)
                    return data;

                WTS_CLIENT_DISPLAY cd = (WTS_CLIENT_DISPLAY)Marshal.PtrToStructure((System.IntPtr)buffer, typeof(WTS_CLIENT_DISPLAY));
                data.ClientDisplay = cd;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ClientHardwareId, out buffer, out bytesReturned);

                if (!worked)
                    return data;

                lData = Marshal.ReadInt32(buffer);
                data.ClientHardwareId = lData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ClientName, out buffer, out bytesReturned);
                strData = Marshal.PtrToStringAnsi(buffer);
                data.ClientName = strData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ClientProductId, out buffer, out bytesReturned);
                Int16 intData = Marshal.ReadInt16(buffer);
                data.ClientProductId = intData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ClientProtocolType, out buffer, out bytesReturned);
                intData = Marshal.ReadInt16(buffer);
                data.ClientProtocolType = intData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.ConnectState, out buffer, out bytesReturned);
                lData = Marshal.ReadInt32(buffer);
                data.ConnectState = (WTS_CONNECTSTATE_CLASS)Enum.ToObject(typeof(WTS_CONNECTSTATE_CLASS), lData);

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.DomainName, out buffer, out bytesReturned);
                strData = Marshal.PtrToStringAnsi(buffer);
                data.DomainName = strData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.InitialProgram, out buffer, out bytesReturned);
                strData = Marshal.PtrToStringAnsi(buffer);
                data.InitialProgram = strData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.OEMId, out buffer, out bytesReturned);
                strData = Marshal.PtrToStringAnsi(buffer);
                data.OEMId = strData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.SessionId, out buffer, out bytesReturned);
                lData = Marshal.ReadInt32(buffer);
                data.SessionId = lData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.UserName, out buffer, out bytesReturned);
                strData = Marshal.PtrToStringAnsi(buffer);
                data.UserName = strData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.WinStationName, out buffer, out bytesReturned);
                strData = Marshal.PtrToStringAnsi(buffer);
                data.WinStationName = strData;

                worked = WTSQuerySessionInformation(server, SessionId,
                    WTS_INFO_CLASS.WorkingDirectory, out buffer, out bytesReturned);
                strData = Marshal.PtrToStringAnsi(buffer);
                data.WorkingDirectory = strData;
            }
            finally
            {
                WTSFreeMemory(buffer);
                buffer = IntPtr.Zero;
                CloseServer(server);
            }

            return data;
        }

        private static string TerminalSessionInfoToString(TerminalSessionInfo termInfo)
        {
            string info = "Application Name: " + termInfo.ApplicationName + "\r\n";
            info += "Client address: " + ClientAddressToString(termInfo.ClientAddress.Address) + "\r\n";
            info += "Client build number: " + termInfo.ClientBuildNumber + "\r\n";
            info += "Client directory: " + termInfo.ClientDirectory + "\r\n";
            info += "Client name: " + termInfo.ClientName + "\r\n";
            info += "Domain name: " + termInfo.DomainName + "\r\n";
            info += "Initial program: " + termInfo.InitialProgram + "\r\n";
            info += "OEMId: " + termInfo.OEMId + "\r\n";
            info += "SessionId: " + termInfo.SessionId + "\r\n";
            info += "UserName: " + termInfo.UserName + "\r\n";
            info += "WinStationName: " + termInfo.WinStationName + "\r\n";
            info += "Working directory: " + termInfo.WorkingDirectory + "\r\n";
            return info;
        }

        private static string ClientAddressToString(byte[] clientAddress)
        {
            byte[] ipBytes = new byte[] { clientAddress[2], clientAddress[3], clientAddress[4], clientAddress[5] };
            IPAddress address = new IPAddress(ipBytes);
            return address.ToString();
        }
    }
}
