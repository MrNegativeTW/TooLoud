using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooLoud.Helpers;
using TooLoud.UI;

namespace TooLoud {
    public class TooLoudHandler : ObservableObject {

        #region Properties

        public static TooLoudHandler Instance { get; set; }

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
        }

        private void OnRunAtStartupChanged() {
            //StartupHelper.SetRunAtStartupEnabled(runAtStartup);
            StartupHelper.SetRunAtStartupEnabled("TooLoud");
        }
    }
}
