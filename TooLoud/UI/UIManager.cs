using CommunityToolkit.Mvvm.ComponentModel;
using Hardcodet.Wpf.TaskbarNotification;
using ModernWpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooLoud.Helpers;
using Windows.Storage;

namespace TooLoud.UI {
    public class UIManager : ObservableObject {

        #region Themes

        private ElementTheme currentSystemTheme = ElementTheme.Dark;

        private bool _isThemeUpdated;

        private bool useColoredTrayIcon = DefaultValuesStore.UseColoredTrayIcon;

        public bool UseColoredTrayIcon {
            get => useColoredTrayIcon;
            set {
                if (SetProperty(ref useColoredTrayIcon, value)) {
                    OnUseColoredTrayIconChanged();
                }
            }
        }
        #endregion

        #region GeneralPage

        private bool protectionEnabled = DefaultValuesStore.ProtectionEnabled;

        public bool ProtectionEnabled {
            get => protectionEnabled;
            set {
                if (SetProperty(ref protectionEnabled, value)) {
                    OnProtectionEnabledChanged();
                }
            }
        }

        private int mainMaximumVolume = DefaultValuesStore.MainMaximumVolume;

        public int MainMaximumVolume {
            get => mainMaximumVolume;
            set {
                if (SetProperty(ref mainMaximumVolume, value)) {
                    OnMainMaximumVolmueChanged(value);
                }
            }
        }

        #endregion

        public void Initialize() {
            //ProtectionEnabled = AppDataHelper.ProtectionEnabled;
            //MainMaximumVolume = AppDataHelper.MainMaximumVolume;
            ProtectionEnabled = AppSettingsHelper.ProtectionEnabled;
            MainMaximumVolume = AppSettingsHelper.MainMaximumVolume;

            TrayIconManager.SetupTrayIcon();

            SystemTheme.SystemThemeChanged += OnSystemThemeChanged;
            SystemTheme.Initialize();
        }
        public static void Dispose() {
            TrayIconManager.Dispose();
        }

        private void OnSystemThemeChanged(object sender, SystemThemeChangedEventArgs args) {
            currentSystemTheme = args.IsSystemLightTheme ? ElementTheme.Light : ElementTheme.Dark;
            UpdateTheme();
        }

        private void UpdateTheme() {
            //ActualFlyoutTheme = flyoutTheme == ElementTheme.Default ? currentSystemTheme : flyoutTheme;

            if (!_isThemeUpdated) {
                _isThemeUpdated = true;
            }

            //UpdateFlyoutBackgroundOpacity();
            UpdateTrayIcon();
        }

        private void OnUseColoredTrayIconChanged() {
            UpdateTrayIcon();
            //AppDataHelper.UseColoredTrayIcon = useColoredTrayIcon;
            AppSettingsHelper.UseColoredTrayIcon = useColoredTrayIcon;
        }

        private void OnProtectionEnabledChanged() {
            //Trace.WriteLine("OnProtectionEnabledChanged() called");
            AppSettingsHelper.ProtectionEnabled = protectionEnabled;
        }

        private void OnMainMaximumVolmueChanged(int volume) {
            //Trace.WriteLine($"OnMainMaximumVolmueChanged({volume}) called");
            AppSettingsHelper.MainMaximumVolume = mainMaximumVolume;
        }

        private void UpdateTrayIcon() {
            if (!_isThemeUpdated) return;

            TrayIconManager.UpdateTrayIconInternal(currentSystemTheme, useColoredTrayIcon);
        }
    }
}