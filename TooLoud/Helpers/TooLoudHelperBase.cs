using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooLoud.Helpers {
    public abstract class TooLoudHelperBase : ObservableObject {

        #region Properties

        private bool isEnabled = true;

        public bool IsEnabled {
            get => isEnabled;
            set {
                if (SetProperty(ref isEnabled, value)) {
                    OnIsEnabledChanged();
                }
            }
        }

        #endregion

        private void OnIsEnabledChanged() {
            if (isEnabled) {
                OnEnabled();
            } else {
                OnDisabled();
            }
        }


        protected virtual void OnEnabled() {
        }

        protected virtual void OnDisabled() {
            //if (FlyoutHandler.Instance.OnScreenFlyoutView.FlyoutHelper == this) {
            //    FlyoutHandler.Instance.OnScreenFlyoutWindow.IsOpen = false;
            //}
        }
    }
}
