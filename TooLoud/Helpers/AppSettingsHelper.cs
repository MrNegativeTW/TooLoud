using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Application Settings (.NET Settings)
/// </summary>
namespace TooLoud.Helpers {
    public static class AppSettingsHelper
    {
        private static readonly Properties.Settings Settings = Properties.Settings.Default;

        #region Theme
        public static bool UseColoredTrayIcon
        {
            get => Settings.UseColoredTrayIcon;
            set
            {
                Settings.UseColoredTrayIcon = value;
                Settings.Save();
            }
        }
        #endregion

        #region General
        //public static bool RunOnStartupEnabled {
        //    get => Settings.RunOnStartupEnabled;
        //    set {
        //        Settings.RunOnStartupEnabled = value;
        //        Settings.Save();
        //    }
        //}

        public static event EventHandler<bool> ProtectionEnabledChanged;

        public static bool ProtectionEnabled
        {
            get => Settings.ProtectionEnabled;
            set
            {
                if (Settings.ProtectionEnabled != value) {
                    Settings.ProtectionEnabled = value;
                    Settings.Save();
                    ProtectionEnabledChanged?.Invoke(null, value);
                }
            }
        }

        public static event EventHandler<int> MainMaximumVolumeChanged;

        public static int MainMaximumVolume
        {
            get => Settings.MainMaximumVolume;
            set
            {
                if (Settings.MainMaximumVolume != value) {
                    Settings.MainMaximumVolume = value;
                    Settings.Save();
                    MainMaximumVolumeChanged?.Invoke(null, value);
                }
            }
        }
        #endregion

        public static void ResetToDefaults()
        {
            Settings.Reset();
            Settings.Save();
        }
    }
}
