using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooLoud.Helpers;
using TooLoud.UI;
using System.Windows;
using Windows.Storage;

namespace TooLoud {
    public class TooLoudHandler : ObservableObject {

        public static event EventHandler Initialized;

        #region Properties

        public static TooLoudHandler Instance { get; set; }

        public static bool HasInitialized { get; private set; }

        public AudioHelper AudioHelper { get; set; }

        public UIManager UIManager { get; set; }

        private bool runAtStartup;

        public bool RunAtStartup {
            get => runAtStartup;
            set {
                if (SetProperty(ref runAtStartup, value)) {
                    OnRunAtStartupChanged();
                }
            }
        }

        #endregion

        public void Initialize() {
            //try {
            //    var packageFamilyName = Windows.ApplicationModel.Package.Current?.Id?.FamilyName;
            //    Trace.WriteLine($"Package Family Name: {packageFamilyName ?? "Not packaged"}");

            //    var localSettingsPath = ApplicationData.Current?.LocalFolder?.Path;
            //    Trace.WriteLine($"LocalSettings Path: {localSettingsPath ?? "Not available"}");

            //    var settingsAvailable = ApplicationData.Current?.LocalSettings != null;
            //    Trace.WriteLine($"LocalSettings Available: {settingsAvailable}");
            //} catch (Exception ex) {
            //    Trace.WriteLine($"App Diagnostics Failed: {ex.Message}");
            //}
            UIManager = new UIManager();
            UIManager.Initialize();

            #region App Data

            var adEnabled = AppDataHelper.ProtectionEnabled;

            async void getStartupStatus() {
                //RunAtStartup = await StartupHelper.GetRunAtStartupEnabled();
                RunAtStartup = StartupHelper.GetRunAtStartupEnabled("TooLoud");
            }
            getStartupStatus();

            #endregion

            #region Initiate Helpers

            AudioHelper = new AudioHelper() { IsEnabled = adEnabled };

            #endregion

            HasInitialized = true;
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        private void OnRunAtStartupChanged() {
            //StartupHelper.SetRunAtStartupEnabled(runAtStartup);
            StartupHelper.SetRunAtStartupEnabled("TooLoud");
        }

        public static void ShowSettingsWindow() {
            Trace.WriteLine("ShowSettingsWindow() called, Not yet implemented");
            //Application.Current.Dispatcher.Invoke(() => {
            //    Instance.SettingsWindow ??= new SettingsWindow();
            //    Instance.SettingsWindow.Show();
            //    Instance.SettingsWindow.Activate();
            //    Instance.SettingsWindow.Focus();
            //});
        }

        public static void SafelyExitApplication() {
            //NativeFlyoutHandler.Instance.ShowNativeFlyout();
            Environment.Exit(0);
        }
    }
}
