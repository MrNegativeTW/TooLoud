using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TooLoud.Pages {
    /// <summary>
    /// Interaction logic for GeneralPage.xaml
    /// </summary>
    public partial class GeneralPage : Page {
        public GeneralPage() {
            InitializeComponent();
        }

        private void MaxVolumeSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            var slider = sender as Slider;
            if (slider != null) {
                if (e.Delta > 0)
                    slider.Value += slider.SmallChange;
                else
                    slider.Value -= slider.SmallChange;

                e.Handled = true;
            }
        }
    }
}
