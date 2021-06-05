using SessionsAPI.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SessionsAPI
{
    public static class Library
    {
        public struct Desktop
        {
            public IntPtr SessionID;
            public String Name;
        }

        public struct ManagedProcess
        {
            public Desktop Desktop;
            public Process Process;
            //internal string Name;
        }

        public static List<Desktop> Desktops = new List<Desktop> { };
        public static List<ManagedProcess> ManagedProcesses = new List<ManagedProcess> { };

        public static Desktop CreateDesktop(string Name, WinAPI.DESKTOP_ACCESS Type = WinAPI.DESKTOP_ACCESS.CUSTOM_SECURE)
        {
            Desktop NewDesktop = new Desktop();
            NewDesktop.Name = Name;
            NewDesktop.SessionID = WinAPI.CreateDesktop(Name, IntPtr.Zero, IntPtr.Zero, 0, (uint)Type, IntPtr.Zero);

            Desktops.Add(NewDesktop);

            return NewDesktop;
        }

        public static bool DestroyDesktop(Desktop Desktop)
        {
            try
            {
                WinAPI.CloseDesktop(Desktop.SessionID);
                Desktops.Remove(Desktop);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void CreateProcess(string Image, Desktop Desktop)
        {
            ManagedProcess NewProcess = new ManagedProcess();
            WinAPI.STARTUPINFO StartupInfomation = new WinAPI.STARTUPINFO();
            WinAPI.PROCESS_INFORMATION ProcessInfomation;

            StartupInfomation.lpDesktop = Desktop.Name;
            StartupInfomation.dwFlags |= 0x00000020;

            WinAPI.CreateProcess(null, Image, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref StartupInfomation, out ProcessInfomation);

            NewProcess.Process = Process.GetProcessById((int)ProcessInfomation.dwProcessId);
            NewProcess.Desktop = Desktop;

            ManagedProcesses.Add(NewProcess);
        }

        public static void SwitchDesktop(Desktop Desktop)
        {
            WinAPI.SwitchDesktop(Desktop.SessionID);
        }
    }
}