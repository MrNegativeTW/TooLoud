using CommunityToolkit.Mvvm.ComponentModel;
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
    }
}