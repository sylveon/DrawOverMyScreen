using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;

class DrawOverMyScreenClient {
    [DllImport("user32.dll")]
    public static extern bool LockWorkStation();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SystemParametersInfo(int uAction, int uParam, ref bool lpvParam, int flags);

    private const int SPI_SETSCREENSAVERACTIVE = 17;
    private const int SPIF_SENDWININICHANGE = 2;

    public static void Main(string[] CommandLine) {
        if (CommandLine.Length == 0 || CommandLine[0] == "/s") {
            while (true) {
                Process[] ScreensaverProcess = Process.GetProcessesByName("DrawOverMyScreen.scr");
                if (ScreensaverProcess.Length == 0) {
                    break;
                }
            }
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DrawOverMyScreen");
            if (Settings != null) {
                string ScreensaverLocation = Settings.GetValue("Screensaver", string.Empty).ToString();
                if (!string.IsNullOrEmpty(ScreensaverLocation) && File.Exists(ScreensaverLocation)) {
                    ProcessStartInfo Screensaver = new ProcessStartInfo("cmd.exe", "/d /c \"" + ScreensaverLocation + "\" " + "/s");
                    Screensaver.WindowStyle = ProcessWindowStyle.Hidden;
                    Process ScreensaverProcess = Process.Start(Screensaver);
                    RegistryKey ControlPanel = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
                    SetScreenSaverActive(0);
                    ScreensaverProcess.WaitForExit();
                    SetScreenSaverActive(1);
                    if (Int32.Parse(ControlPanel.GetValue("ScreensaverIsSecure", string.Empty).ToString()) == 1) {
                        LockWorkStation();
                    }
                }
            }
        } else if (CommandLine[0] == "/register") {
            using(TaskService ScheduledTasks = new TaskService()) {
                try {
                    ScheduledTasks.RootFolder.DeleteTask("DrawOverMyScreen-" + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                } catch (Exception) {}
                TaskDefinition TaskProperties = ScheduledTasks.NewTask();
                TaskProperties.Actions.Add(new ExecAction(Process.GetCurrentProcess().MainModule.FileName, "/s", null));
                TaskProperties.Settings.DisallowStartIfOnBatteries = false;
                ScheduledTasks.RootFolder.RegisterTaskDefinition("DrawOverMyScreen-" + System.Security.Principal.WindowsIdentity.GetCurrent().Name, TaskProperties);
            }
        } else if (CommandLine[0] == "/unregister") {
            using(TaskService ScheduledTasks = new TaskService()) {
                try {
                    ScheduledTasks.RootFolder.DeleteTask("DrawOverMyScreen-" + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                } catch (Exception) {}
            }
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
            Software.DeleteSubKey("DrawOverMyScreen");
        }
    }

    public static void SetScreenSaverActive(int Active) {
        bool nullVar = false;
        SystemParametersInfo(SPI_SETSCREENSAVERACTIVE, Active, ref nullVar, SPIF_SENDWININICHANGE);
    }
}