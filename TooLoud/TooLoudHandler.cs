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

        // ...
        public UIManager UIManager { get; set; }

        #endregion

        public void Initialize() {
            UIManager = new UIManager();
            UIManager.Initialize();

            #region App Data

            var adEnabled = AppDataHelper.ProtectionEnabled;

            #endregion

            #region Initiate Helpers

            AudioHelper = new AudioHelper() { IsEnabled = adEnabled };

            #endregion
        }
    }
}
