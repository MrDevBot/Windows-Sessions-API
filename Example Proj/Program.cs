using System.Threading;
using System.Windows.Forms;
using SessionsAPI;
using SessionsAPI.Win32;

namespace SecureDesktop
{
    class Program
    {
        static void Main(string[] args)
        {
            Library.Desktop CurrentDesktop = new Library.Desktop();
            CurrentDesktop.SessionID = WinAPI.GetThreadDesktop(WinAPI.GetCurrentThreadId());
            //CurrentDesktop.Name = ...

            Library.Desktop NewDesktop = Library.CreateDesktop("MyCustomDesktop", WinAPI.DESKTOP_ACCESS.CUSTOM_SECURE);

            Library.SwitchDesktop(NewDesktop);

            Library.CreateProcess("C:\\windows\\regedit.exe", NewDesktop);

            Thread.Sleep(5000);

            #region Cleanup

            Library.SwitchDesktop(CurrentDesktop);

            string KillList = "";
            foreach(Library.ManagedProcess Obj in Library.ManagedProcesses)
            {
                KillList += string.Format(" {0}", Obj.Process.ProcessName); 
                Obj.Process.Kill();
            }

            MessageBox.Show(string.Format("Killing Processes: {0}", KillList));

            #endregion
        }
    }
}
