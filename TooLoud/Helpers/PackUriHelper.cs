using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooLoud.Helpers {
    internal static class PackUriHelper {
        public static Uri GetAbsoluteUri(string path) {
            return new Uri($"pack://application:,,,/TooLoud;component/{path}");
        }
    }
}
