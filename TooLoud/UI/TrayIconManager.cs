using Hardcodet.Wpf.TaskbarNotification;
using ModernWpf;
using ModernWpf.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TooLoud.Helpers;
using TooLoud.Utils;

namespace TooLoud.UI {
    internal class TrayIconManager {

        public static ContextMenu TaskbarIconContextMenu { get; private set; }

        public static ToolTip TaskbarIconToolTip { get; private set; }

        public static TaskbarIcon TaskbarIcon { get; private set; }

        public static void SetupTrayIcon() {
            var settingsItem = new MenuItem() {
                Header = "Settings",
                //ToolTip = "Properties.Strings.SettingsItemDescription",
                Icon = new FontIcon() { Glyph = CommonGlyphs.Settings },
                Command = CommonCommands.OpenSettingsWindowCommand
            };

            var exitItem = new MenuItem() {
                Header = "Quit TooLoud",
                //ToolTip = "Properties.Strings.ExitItemDescription",
                Icon = new FontIcon() { Glyph = CommonGlyphs.PowerButton },
                Command = CommonCommands.ExitAppCommand
            };

            TaskbarIconContextMenu = new ContextMenu() {
                Items = { settingsItem, exitItem }
            };

            TaskbarIconToolTip = new ToolTip() { Content = DefaultValuesStore.AppName };

            TaskbarIcon = new TaskbarIcon() {
                TrayToolTip = TaskbarIconToolTip,
                ContextMenu = TaskbarIconContextMenu,
                DoubleClickCommand = CommonCommands.OpenSettingsWindowCommand
            };
        }

        /**
         * <summary>Show/Hide the tray icon, configurable by user.</summary>
         * <param name="isVisible">The visibility of the tray icon.</param>
         */
        public static void UpdateTrayIconVisibility(bool isVisible) {
            TaskbarIcon.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public static void UpdateTrayIconInternal(ElementTheme currentTheme, bool useColoredTrayIcon) {
            ThemeManager.SetRequestedTheme(TaskbarIconContextMenu, currentTheme);
            ThemeManager.SetRequestedTheme(TaskbarIconToolTip, currentTheme);

            Uri iconUri;
            if (useColoredTrayIcon) {
                iconUri = PackUriHelper.GetAbsoluteUri(@"Assets\Logo.ico");
            } else {
                iconUri = Helpers.PackUriHelper.GetAbsoluteUri(currentTheme == ElementTheme.Light ? @"Assets\Logo_Tray_Black.ico" : @"Assets\Logo_Tray_White.ico");
            }

            TaskbarIcon.IconSource = BitmapFrame.Create(iconUri);
        }
    }
}
