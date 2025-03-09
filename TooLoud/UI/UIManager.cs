using CommunityToolkit.Mvvm.ComponentModel;
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

        private int mainMaximunVolumn = DefaultValuesStore.MainMaximunVolumn;

        public int MainMaximunVolumn {
            get => mainMaximunVolumn;
            set {
                if (SetProperty(ref mainMaximunVolumn, value)) {
                    OnMainMaximunVolmunChanged(value);
                }
            }
        }

        #endregion

        public void Initialize() {
            ProtectionEnabled = AppDataHelper.ProtectionEnabled;
            MainMaximunVolumn = AppDataHelper.MainMaximunVolumn;

            TrayIconManager.SetupTrayIcon();

            SystemTheme.SystemThemeChanged += OnSystemThemeChanged;
            SystemTheme.Initialize();
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
            AppDataHelper.UseColoredTrayIcon = useColoredTrayIcon;
        }

        private void OnProtectionEnabledChanged() {
            Trace.WriteLine("OnProtectionEnabledChanged() called");
            AppDataHelper.ProtectionEnabled = protectionEnabled;
        }

        private void OnMainMaximunVolmunChanged(int volumn) {
            Trace.WriteLine($"OnMainMaximunVolmunChanged({volumn}) called");
            //UpdateFlyoutBackgroundOpacity();
            AppDataHelper.MainMaximunVolumn = mainMaximunVolumn;
        }

        private void UpdateTrayIcon() {
            if (!_isThemeUpdated) return;

            TrayIconManager.UpdateTrayIconInternal(currentSystemTheme, useColoredTrayIcon);
        }
    }
}