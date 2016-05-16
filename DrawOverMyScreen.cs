using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

public class DrawOverMyScreen {
  [STAThread]
  public static void Main(string[] CommandLine) {
    if (CommandLine.Length == 0 || CommandLine[0].IndexOf("/c", StringComparison.OrdinalIgnoreCase) >= 0) {
      switch (MessageBox.Show("What do you want to do?\n\n - Press \"Yes\" to configure the screensaver\n - Press \"No\" to change the screensaver\n - Press \"Cancel\" to do nothing", "DrawOverMyScreen Configuration", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)) {
        case DialogResult.Yes:
          Screensaver("/c");
          break;

        case DialogResult.No:
          OpenFileDialog BrowseScreensaver = new OpenFileDialog();
          BrowseScreensaver.Filter = "Screensavers|*.scr";
          BrowseScreensaver.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
          if (BrowseScreensaver.ShowDialog() == DialogResult.OK) {
            RegistryKey Settings = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DrawOverMyScreen", true);
            Settings.SetValue("Screensaver", BrowseScreensaver.FileName);
          }
          break;

        default:
          break;
      }
    } else if (string.Equals(CommandLine[0], "/p", StringComparison.CurrentCultureIgnoreCase)) {
      Screensaver(CommandLine[0] + " " + CommandLine[1]);
    } else if (string.Equals(CommandLine[0], "/s", StringComparison.CurrentCultureIgnoreCase)) {
      Screensaver("/s");
    }
  }


  public static void Screensaver(string CommandLine) {
    if (CommandLine == "/s") {
      ProcessStartInfo ScheduledTask = new ProcessStartInfo("schtasks", "/run /tn DrawOverMyScreen-" + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
      ScheduledTask.WindowStyle = ProcessWindowStyle.Hidden;
      Process.Start(ScheduledTask);
    } else {
      RegistryKey Settings = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DrawOverMyScreen");
      if (Settings != null) {
        string ScreensaverLocation = Settings.GetValue("Screensaver", string.Empty).ToString();
        if (!string.IsNullOrEmpty(ScreensaverLocation) && File.Exists(ScreensaverLocation)) {
          ProcessStartInfo Screensaver = new ProcessStartInfo("cmd.exe", "/d /c \"" + ScreensaverLocation + "\" " + CommandLine);
          Screensaver.WindowStyle = ProcessWindowStyle.Hidden;
          Process ScreensaverProcess = Process.Start(Screensaver);
          ScreensaverProcess.WaitForExit();
        }
      }
    }
  }
}