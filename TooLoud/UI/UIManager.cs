using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //private bool runOnStartupEnabled = DefaultValuesStore.RunOnStartupEnabled;
        private bool runOnStartupEnabled = false;

        public bool RunOnStartupEnabled {
            get => runOnStartupEnabled;
            set {
                if (SetProperty(ref runOnStartupEnabled, value)) {
                    OnRunOnStartupChanged();
                }
            }
        }

        private int mainMaximunVolumn = 20;

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

        }

        private void OnRunOnStartupChanged() {
            //UpdateFlyoutBackgroundOpacity();
            //AppDataHelper.FlyoutBackgroundOpacity = flyoutBackgroundOpacity;
        }

        private void OnMainMaximunVolmunChanged() {
            //UpdateFlyoutBackgroundOpacity();
            //AppDataHelper.FlyoutBackgroundOpacity = flyoutBackgroundOpacity;
        }
    }
}