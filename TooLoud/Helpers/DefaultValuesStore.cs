using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooLoud.Helpers {
    internal class DefaultValuesStore {

        #region General

        public const string AppName = "TooLoud";

        public const bool RunOnStartupEnabled = false;

        public const bool ProtectionEnabled = true;

        public const int MainMaximunVolumn = 10;

        #endregion

        #region Theme

        // aka Colorful Tray Icon, not B&W.
        public const bool UseColoredTrayIcon = true;

        #endregion
    }
}
