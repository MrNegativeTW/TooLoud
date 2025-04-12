using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace TooLoud.Helpers {
    internal class StartupHelper {
        
        private const string StartupId = "TooLoudStartupId"; // Also appeas in Package.appxmanifest
        private const string APP_NAME = "TooLoud";
        private const string REG_KEY_AUTORUN = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        #region UWP (I don't want to install the UWP SDK, takes so many space)

        //public static async Task<bool> GetRunAtStartupEnabled() {
        //    try {
        //        StartupTask startupTask = await StartupTask.GetAsync(StartupId);

        //        return startupTask.State == StartupTaskState.Enabled;
        //    } catch (Exception ex) { 
        //        Trace.WriteLine($"GetRunAtStartupEnabled failed. {ex.Message}\n{ex.StackTrace}");
        //        return true; 
        //    }
        //}

        //public static async void SetRunAtStartupEnabled(bool value) {
        //    try {
        //        StartupTask startupTask = await StartupTask.GetAsync(StartupId);

        //        if (value) {
        //            await startupTask.RequestEnableAsync();
        //        } else {
        //            startupTask.Disable();
        //        }
        //    } catch (Exception ex) {
        //        Trace.WriteLine($"SetRunAtStartupEnabled failed. {ex.Message}\n{ex.StackTrace}");
        //    }
        //}

        #endregion

        #region WPF (Tranditional way)

        // regedit, path: "\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"
        public static bool GetRunAtStartupEnabled() {
            try {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REG_KEY_AUTORUN, true)) {
                    return key.GetValue(APP_NAME) != null;
                }
            } catch (Exception ex) {
                Trace.WriteLine($"GetRunAtStartupEnabled failed. {ex.Message}\n{ex.StackTrace}");
                return true;
            }
        }

        public static void SetRunAtStartupEnabled(bool enable) {
            try {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REG_KEY_AUTORUN, true)) {
                    if (key == null) {
                        Trace.WriteLine("Failed to open registry key");
                        return;
                    }

                    string exePath = Process.GetCurrentProcess().MainModule?.FileName;
                    if (string.IsNullOrEmpty(exePath)) {
                        Trace.WriteLine("Failed to get executable path");
                        return;
                    }

                    if (enable) {
                        key.SetValue(APP_NAME, exePath);
                    } else {
                        if (key.GetValue(APP_NAME) != null) {
                            key.DeleteValue(APP_NAME, false);
                        }
                    }
                }
            } catch (Exception ex) {
                Trace.WriteLine($"SetRunAtStartupEnabled failed. {ex.Message}\n{ex.StackTrace}");
            }
        }

        #endregion
    }
}
