using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

/// <summary>
/// Copied from ModernWpf.Helpers
/// 
/// Not work in this case, fuck it whatever, use AppSettingsHelper instead.
/// 
/// Windows.Storage.ApplicationData (UWP/WinRT style)
/// </summary>
namespace TooLoud.Helpers {
    public class AppDataHelper {

        #region Methods

        internal static T GetValue<T>(T defaultValue, [CallerMemberName] string propertyName = "") {
            try {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(propertyName)) {
                    string value = ApplicationData.Current.LocalSettings.Values[propertyName].ToString() ?? string.Empty;

                    if (!string.IsNullOrEmpty(value)) {
                        if (typeof(T) == typeof(string)) {
                            return (T)(object)value;
                        } else if (typeof(T) == typeof(bool)) {
                            if (bool.TryParse(value, out bool result)) {
                                return (T)(object)result;
                            }
                        } else if (typeof(T) == typeof(int)) {
                            if (int.TryParse(value, out int result)) {
                                return (T)(object)result;
                            }
                        } else if (typeof(T) == typeof(double)) {
                            if (double.TryParse(value, out double result)) {
                                return (T)(object)result;
                            }
                        } else if (typeof(T).IsEnum) {
                            return (T)Enum.Parse(typeof(T), value);
                        }
                        //else if (typeof(T) == typeof(BindablePoint)) {
                        //    if (BindablePoint.TryParse(value, out BindablePoint result)) {
                        //        return (T)(object)result;
                        //    }
                        //}
                    }
                }
            } catch (Exception ex) {
                //Trace.WriteLine($"GetValue({propertyName}) failed: {ex.Message}");
                // TODO: ApplicationData unusable
            }

            return defaultValue;
        }

        internal static void SetValue<T>(T value, [CallerMemberName] string propertyName = "") {
            try {
                ApplicationData.Current.LocalSettings.Values[propertyName] = value.ToString();
            } catch (Exception ex) {
                //Trace.WriteLine($"SetValue({propertyName}) failed: {ex.Message}");
                // TODO: ApplicationData unusable
            }
        }

        internal static async Task ClearAppDataAsync() {
            try {
                Trace.WriteLine("ClearAppDataAsync called");
                //await Windows.Storage.ApplicationData.Current.ClearAsync();
            } catch (Exception ex) {
                //Trace.WriteLine($"ClearAppDataAsync failed: {ex.Message}");
                // TODO: ApplicationData unusable
            }
        }

        internal static void SavePropertyValue(string value, string propertyName = "") {
            try {
                ApplicationData.Current.LocalSettings.Values[propertyName] = value;
            } catch (Exception ex) {
                //Trace.WriteLine($"SavePropertyValue({propertyName}) failed: {ex.Message}");
                // TODO: ApplicationData unusable
            }
        }

        #endregion

        #region Properties

        #region Theme
        public static bool UseColoredTrayIcon {
            get => GetValue(DefaultValuesStore.UseColoredTrayIcon);
            set => SetValue(value);
        }

        #endregion

        #region General

        public static bool RunOnStartupEnabled {
            get => GetValue(DefaultValuesStore.RunOnStartupEnabled);
            set => SetValue(value);
        }

        public static bool ProtectionEnabled {
            get => GetValue(DefaultValuesStore.ProtectionEnabled);
            set => SetValue(value);
        }

        public static int MainMaximumVolume {
            get => GetValue(DefaultValuesStore.MainMaximumVolume);
            set => SetValue(value);
        }

        #endregion

        #region Placeholder

        //...

        #endregion

        #endregion
    }
}
