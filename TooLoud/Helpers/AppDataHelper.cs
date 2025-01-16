using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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
            } catch { }

            return defaultValue;
        }

        internal static void SetValue<T>(T value, [CallerMemberName] string propertyName = "") {
            try {
                ApplicationData.Current.LocalSettings.Values[propertyName] = value.ToString();
            } catch { }
        }

        internal static async Task ClearAppDataAsync() {
            try {
                Trace.WriteLine("ClearAppDataAsync called");
                //await ApplicationData.Current.ClearAsync();
            } catch { }
        }

        internal static void SavePropertyValue(string value, string propertyName = "") {
            try {
                ApplicationData.Current.LocalSettings.Values[propertyName] = value;
            } catch { }
        }

        #endregion
    }
}
