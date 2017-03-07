using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileDownload.Converters
{
  /// <summary>
  /// format file byte unit to display string.
  /// </summary>
  [ValueConversion(typeof(long), typeof(string))]
  public class FileSizeByteUnitConverter : IValueConverter
  {
    static string[] UNITS = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double size = (long)value;
      int unit = 0;
      while (size >= 1024)
      {
        size /= 1024;
        unit++;
      }
      return String.Format("{0:0.#}{1}", size, UNITS[unit]);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
