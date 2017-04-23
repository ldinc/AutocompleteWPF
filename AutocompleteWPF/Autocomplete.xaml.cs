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

    #endregion Watermark

    #region LeftMark

    public static readonly DependencyProperty LeftMarkProperty =
      DependencyProperty.Register("LeftMark", typeof(object), typeof(Autocomplete), new UIPropertyMetadata(null));

    public object LeftMark {
      get {
        return GetValue(LeftMarkProperty);
      }

      set {
        SetValue(LeftMarkProperty, value);
      }
    }

    #endregion LeftMark

    #region RightMark

    public static readonly DependencyProperty RightMarkProperty =
      DependencyProperty.Register("RightMark", typeof(object), typeof(Autocomplete), new UIPropertyMetadata(null));

    public object RightMark {
      get {
        return GetValue(RightMarkProperty);
      }

      set {
        SetValue(RightMarkProperty, value);
      }
    }

    #endregion RightMark

    #region CornerRadius

    public static readonly DependencyProperty CornerRadiusProperty =
      DependencyProperty.Register("CornerRadius", typeof(int), typeof(Autocomplete), new UIPropertyMetadata(0));

    public int CornerRadius {
      get {
        return (int)GetValue(CornerRadiusProperty);
      }

      set {
        SetValue(CornerRadiusProperty, value);
      }
    }

    #endregion CornerRadius

    #endregion DependencyProperty

    public Autocomplete() {
      InitializeComponent();
    }
  }
}
