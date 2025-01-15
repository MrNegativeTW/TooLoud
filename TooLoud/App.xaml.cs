using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TooLoud {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            InitializeComponent();
            Startup += App_Startup;
            //JumpListHelper.CreateJumpList();
        }

        private void App_Startup(object sender, StartupEventArgs e) {
            TooLoudHandler.Instance = new TooLoudHandler();
            TooLoudHandler.Instance.Initialize();
        }
    }
}
