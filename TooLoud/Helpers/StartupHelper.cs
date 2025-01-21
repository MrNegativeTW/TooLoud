using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace TooLoud.Helpers {
    internal class StartupHelper {
        
        private const string StartupId = "TooLoudStartupId"; // Also appeas in Package.appxmanifest

        #region UWP (I don't want to install the UWP SDK, takes so many space)

        public static async Task<bool> GetRunAtStartupEnabled() {
            try {
                StartupTask startupTask = await StartupTask.GetAsync(StartupId);

                return startupTask.State == StartupTaskState.Enabled;
            } catch (Exception ex) { 
                Trace.WriteLine($"GetRunAtStartupEnabled failed. {ex.Message}\n{ex.StackTrace}");
                return true; 
            }
        }

        public static async void SetRunAtStartupEnabled(bool value) {
            try {
                StartupTask startupTask = await StartupTask.GetAsync(StartupId);

                if (value) {
                    await startupTask.RequestEnableAsync();
                } else {
                    startupTask.Disable();
                }
            } catch (Exception ex) {
                Trace.WriteLine($"SetRunAtStartupEnabled failed. {ex.Message}\n{ex.StackTrace}");
            }
        }

        #endregion

        #region WPF (Tranditional way)

        // regedit, path: "電腦\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"
        public static bool GetRunAtStartupEnabled(string appName) {
            try {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                    return key.GetValue(appName) != null;
                }
            } catch (Exception ex) {
                Trace.WriteLine($"GetRunAtStartupEnabled failed. {ex.Message}\n{ex.StackTrace}");
                return true;
            }
        }

        public static void SetRunAtStartupEnabled(string appName) {
            try {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                    if (key.GetValue(appName) != null) {
                        key.DeleteValue(appName, false);
                    } else {
                        key.SetValue(appName, Process.GetCurrentProcess().MainModule.FileName);
                    }
                }
            } catch (Exception ex) {
                Trace.WriteLine($"SetRunAtStartupEnabled failed. {ex.Message}\n{ex.StackTrace}");
            }
        }

        #endregion
    }
}
