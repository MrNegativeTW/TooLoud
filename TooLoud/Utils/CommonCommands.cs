using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooLoud.Utils {
    public static class CommonCommands {
        public static RelayCommand OpenSettingsWindowCommand { get; } =
            new RelayCommand(() => TooLoudHandler.ShowSettingsWindow(), () => TooLoudHandler.HasInitialized);

        public static RelayCommand ExitAppCommand { get; } =
            new RelayCommand(() => TooLoudHandler.SafelyExitApplication(), () => TooLoudHandler.HasInitialized);

        static CommonCommands() {
            TooLoudHandler.Initialized += (_, __) => Refresh();
        }

        private static void Refresh() {
            OpenSettingsWindowCommand.NotifyCanExecuteChanged();
            //AlignOnScreenFlyoutToDefaultPosition.NotifyCanExecuteChanged();
            //PinUnpinFlyoutTopBarCommand.NotifyCanExecuteChanged();
            //CloseFlyoutCommand.NotifyCanExecuteChanged();
            ExitAppCommand.NotifyCanExecuteChanged();
            //ResetAppDataCommand.NotifyCanExecuteChanged();
        }
    }
}
