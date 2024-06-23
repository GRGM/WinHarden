﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;



namespace InformationUtils
{
    internal static class WindowsSessionUtils
    {
        [DllImport("netapi32.dll", SetLastError = true)]
        private static extern int NetSessionEnum(
        [In, MarshalAs(UnmanagedType.LPWStr)] string ServerName,
        [In, MarshalAs(UnmanagedType.LPWStr)] string UncClientName,
        [In, MarshalAs(UnmanagedType.LPWStr)] string UserName,
        Int32 Level,
        out IntPtr bufptr,
        int prefmaxlen,
        ref Int32 entriesread,
        ref Int32 totalentries,
        ref Int32 resume_handle);

        [StructLayout(LayoutKind.Sequential)]
        public struct SESSION_INFO_10
        {
            /// <summary>
            /// Unicode string specifying the name of the computer that established the session.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string sesi10_cname;
            /// <summary>
            /// <value>Unicode string specifying the name of the user who established the session.</value>
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string sesi10_username;
            /// <summary>
            /// <value>Specifies the number of seconds the session has been active. </value>
            /// </summary>
            public uint sesi10_time;
            /// <summary>
            /// <value>Specifies the number of seconds the session has been idle.</value>
            /// </summary>
            public uint sesi10_idle_time;
        }


        public enum NERR
        {
            /// <summary>
            /// Operation was a success.
            /// </summary>
            NERR_Success = 0,
            /// <summary>
            /// More data available to read. dderror getting all data.
            /// </summary>
            ERROR_MORE_DATA = 234,
            /// <summary>
            /// Network browsers not available.
            /// </summary>
            ERROR_NO_BROWSER_SERVERS_FOUND = 6118,
            /// <summary>
            /// LEVEL specified is not valid for this call.
            /// </summary>
            ERROR_INVALID_LEVEL = 124,
            /// <summary>
            /// Security context does not have permission to make this call.
            /// </summary>
            ERROR_ACCESS_DENIED = 5,
            /// <summary>
            /// Parameter was incorrect.
            /// </summary>
            ERROR_INVALID_PARAMETER = 87,
            /// <summary>
            /// Out of memory.
            /// </summary>
            ERROR_NOT_ENOUGH_MEMORY = 8,
            /// <summary>
            /// Unable to contact resource. Connection timed out.
            /// </summary>
            ERROR_NETWORK_BUSY = 54,
            /// <summary>
            /// Network Path not found.
            /// </summary>
            ERROR_BAD_NETPATH = 53,
            /// <summary>
            /// No available network connection to make call.
            /// </summary>
            ERROR_NO_NETWORK = 1222,
            /// <summary>
            /// Pointer is not valid.
            /// </summary>
            ERROR_INVALID_HANDLE_STATE = 1609,
            /// <summary>
            /// Extended Error.
            /// </summary>
            ERROR_EXTENDED_ERROR = 1208,
            /// <summary>
            /// Base.
            /// </summary>
            NERR_BASE = 2100,
            /// <summary>
            /// Unknown Directory.
            /// </summary>
            NERR_UnknownDevDir = (NERR_BASE + 16),
            /// <summary>
            /// Duplicate Share already exists on server.
            /// </summary>
            NERR_DuplicateShare = (NERR_BASE + 18),
            /// <summary>
            /// Memory allocation was to small.
            /// </summary>
            NERR_BufTooSmall = (NERR_BASE + 23)
        }


        internal static string EnumSessionsToString(string server)
        {
            string result = "";
            SESSION_INFO_10[] sessions = EnumSessions10(server);
            foreach (SESSION_INFO_10 session in sessions)
            {
                string sessionString = SessionInfo10ToString(session);
                result += sessionString + "\r\n";
            }
            return result;
        }
        private static string SessionInfo10ToString(SESSION_INFO_10 session)
        {
            string info = "Username: " + session.sesi10_username + "\r\n";
            info += "\tComputer name: " + session.sesi10_cname + "\r\n";
            info += "\tIdle time: " + session.sesi10_idle_time + "\r\n";
            info += "\tTime: " + session.sesi10_time + "\r\n";
            return info;
        }

        private static SESSION_INFO_10[] EnumSessions10(string server)
        {
            IntPtr BufPtr;
            int res = 0;
            Int32 er = 0, tr = 0, resume = 0;
            BufPtr = (IntPtr)Marshal.SizeOf(typeof(SESSION_INFO_10));
            SESSION_INFO_10[] results = new SESSION_INFO_10[0];
            do
            {
                res = NetSessionEnum(server, null, null, 10, out BufPtr, -1, ref er, ref tr, ref resume);
                results = new SESSION_INFO_10[er];
                if (res == (int)NERR.ERROR_MORE_DATA || res == (int)NERR.NERR_Success)
                {

                    var p = (Int64)0;
                    if (Environment.Is64BitProcess)
                    {
                        p = BufPtr.ToInt64();
                    }
                    else
                    {
                        p = BufPtr.ToInt32();
                    }
                    for (int i = 0; i < er; i++)
                    {

                        SESSION_INFO_10 si = (SESSION_INFO_10)Marshal.PtrToStructure(new IntPtr(p), typeof(SESSION_INFO_10));
                        results[i] = si;
                        p += Marshal.SizeOf(typeof(SESSION_INFO_10));
                    }
                }
                Marshal.FreeHGlobal(BufPtr);
            }
            while (res == (int)NERR.ERROR_MORE_DATA);
            return results;
        }





    }
}
