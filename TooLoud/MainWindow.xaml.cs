using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TooLoud.Pages;

namespace TooLoud {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private const string TAG = "MainWindow";

        private bool _isActive;

        public MainWindow() {
            InitializeComponent();

            ContentFrame.Navigated += OnNavigated;

            NavView_Navigate("general", new EntranceNavigationTransitionInfo());
        }

        protected override void OnActivated(EventArgs e) {
            if (!_isActive) {
                //    Workarounds.RenderLoopFix.ApplyFix();
                _isActive = true;
            }

            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e) {
            _isActive = false;

            base.OnDeactivated(e);
        }

        protected override void OnClosing(CancelEventArgs e) {
            //AppDataHelper.SettingsWindowPlacement = WindowPlacementHelper.GetPlacement(new WindowInteropHelper(this).Handle);
            e.Cancel = true;
            Hide();

            base.OnClosing(e);
        }

        #region Navigation

        private readonly List<Tuple<string, Type>> _pages = new List<Tuple<string, Type>>() {
            Tuple.Create("general", typeof(GeneralPage)),
            Tuple.Create("placeholder", typeof(PlaceholderPage)), 
            Tuple.Create("about", typeof(AboutPage)),
        };

        private void NavView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args) {
            if (args.SelectedItem != null) {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavView_Navigate(string navItemTag, NavigationTransitionInfo info) {
            //Trace.WriteLine($"{TAG} NavView_Navigate: {navItemTag}");
            var item = _pages.FirstOrDefault(p => p.Item1.Equals(navItemTag));
            Type pageType = item.Item2;

            if (pageType != null && ContentFrame.CurrentSourcePageType != pageType) {
                ContentFrame.Navigate(pageType, null, info);
            }
        }

        private void NavView_BackRequested(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewBackRequestedEventArgs args) {
            BackRequested();
        }

        private bool BackRequested() {
            Trace.WriteLine("BackRequested");
            //if (!ContentFrame.CanGoBack) return false;

            //if (NavView.IsPaneOpen &&
            //    (NavView.DisplayMode == NavigationViewDisplayMode.Minimal
            //     || NavView.DisplayMode == NavigationViewDisplayMode.Compact)) {
            //    return false;
            //}

            //ContentFrame.GoBack();
            return true;
        }

        private void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e) {
            //NavView.IsBackEnabled = ContentFrame.CanGoBack;
            //Type sourcePageType = ContentFrame.SourcePageType;
            //if (sourcePageType != null) {
            //    var item = _pages.FirstOrDefault(p => p.PageType == sourcePageType);

            //    NavView.SelectedItem = NavView.FooterMenuItems
            //        .OfType<NavigationViewItem>().
            //        FirstOrDefault(n => n.Tag.Equals(item.Tag)) ??
            //        NavView.MenuItems
            //        .OfType<NavigationViewItem>()
            //        .FirstOrDefault(n => n.Tag.Equals(item.Tag));

                HeaderBlock.Text =
                    ((NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
            //}
        }

            #endregion
    }
}
