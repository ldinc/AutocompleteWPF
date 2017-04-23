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

namespace AutocompleteWPF {
  /// <summary>
  /// Interaction logic for Autocomplete.xaml
  /// </summary>
  public partial class Autocomplete : UserControl {

    #region DependencyProperty

    #region Watermark

    public static readonly DependencyProperty WatermarkProperty =
      DependencyProperty.Register("Watermark", typeof(string), typeof(Autocomplete), new UIPropertyMetadata(string.Empty));

    public string Watermark {
      get {
        return (string)GetValue(WatermarkProperty);
      }

      set {
        SetValue(WatermarkProperty, value);
      }
    }

    #endregion

    #endregion

    public Autocomplete() {
      InitializeComponent();
    }
  }
}
