using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutocompleteWPF {
  public class StringToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      string text = value as string;
      if (text == null) {
        return Visibility.Collapsed;
      }
      if (string.IsNullOrEmpty(text)) {
        return Visibility.Collapsed;
      }
      return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class NotStringToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      string text = value as string;
      if (text == null) {
        return Visibility.Visible;
      }
      if (string.IsNullOrEmpty(text)) {
        return Visibility.Visible;
      }
      return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

}
