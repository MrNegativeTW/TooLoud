using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooLoud.Helpers;

namespace TooLoud.UI {
    public class UIManager : ObservableObject {

        #region GeneralPage

        //private double flyoutBackgroundOpacity = DefaultValuesStore.FlyoutBackgroundOpacity;

        //public double FlyoutBackgroundOpacity {
        //    get => flyoutBackgroundOpacity;
        //    set {
        //        if (SetProperty(ref flyoutBackgroundOpacity, value)) {
        //            OnFlyoutBackgroundOpacityChanged();
        //        }
        //    }
        //}

        private bool runOnStartupEnabled = DefaultValuesStore.RunOnStartupEnabled;

        public bool RunOnStartupEnabled {
            get => runOnStartupEnabled;
            set {
                if (SetProperty(ref runOnStartupEnabled, value)) {
                    OnRunOnStartupChanged();
                }
            }
        }

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
                    OnMainMaximunVolmunChanged();
                }
            }
        }

        #endregion

        public void Initialize() {
            RunOnStartupEnabled = AppDataHelper.RunOnStartupEnabled;
            ProtectionEnabled = AppDataHelper.ProtectionEnabled;
            MainMaximunVolumn = AppDataHelper.MainMaximunVolumn;
        }

        private void OnRunOnStartupChanged() {
            //TrayIconManager.UpdateTrayIconVisibility(trayIconEnabled);
            Trace.WriteLine("OnRunOnStartupChanged() called");
            AppDataHelper.RunOnStartupEnabled = RunOnStartupEnabled;
        }

        private void OnProtectionEnabledChanged() {
            Trace.WriteLine("OnProtectionEnabledChanged() called");
            AppDataHelper.ProtectionEnabled = ProtectionEnabled;
        }

        private void OnMainMaximunVolmunChanged() {
            Trace.WriteLine("OnMainMaximunVolmunChanged() called");
            //UpdateFlyoutBackgroundOpacity();
            //AppDataHelper.FlyoutBackgroundOpacity = flyoutBackgroundOpacity;
        }
    }
}