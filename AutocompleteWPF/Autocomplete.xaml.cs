using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

    #region SuggestEngine

    public static readonly DependencyProperty SuggestEngineProperty =
      DependencyProperty.Register("SuggestEngine", typeof(ICanSuggestCompletion), typeof(Autocomplete), new UIPropertyMetadata(null));

    public ICanSuggestCompletion SuggestEngine {
      get {
        return (ICanSuggestCompletion)GetValue(SuggestEngineProperty);
      }

      set {
        SetValue(SuggestEngineProperty, value);
      }
    }

    #endregion SuggestEngine

    #region Suggestions

    public static readonly DependencyProperty SuggestionsProperty =
      DependencyProperty.Register("Suggestions", typeof(IEnumerable<object>), typeof(Autocomplete), new UIPropertyMetadata(null));

    public IEnumerable<object> Suggestions {
      get {
        return (IEnumerable<object>)GetValue(SuggestionsProperty);
      }

      set {
        SetValue(SuggestionsProperty, value);
      }
    }

    #endregion Suggestions

    #endregion DependencyProperty

    #region InputMappings

    private Popup popup = null;
    private Popup Popup {
      get {
        if (popup == null) {
          popup = FindChild<Popup>(this, "_popup");
        }
        return popup;
      }
    }

    private TextBox input = null;
    private TextBox Input {
      get {
        if (input == null) {
          input = FindChild<TextBox>(this, "_input");
        }
        return input;
      }
    }

    private ScrollViewer scroll = null;
    private ScrollViewer Scroll {
      get {
        if (scroll == null) {
          scroll = FindChild<ScrollViewer>(Popup, "_scroll");
        }
        return scroll;
      }
    }

    private ListBox lstbox = null;
    private ListBox ListBox {
      get {
        if (lstbox == null) {
          lstbox = FindChild<ListBox>(Scroll, "_lstbox");
        }
        return lstbox;
      }
    }

    #endregion InputMappings

    public Autocomplete() {
      InitializeComponent();
      LostFocus += Autocomplete_LostFocus;
      GotFocus += Autocomplete_GotFocus;
      ListBox.MouseDown += _lstbox_MouseDown;
      //MouseDown += Autocomplete_MouseDown;
    }

    private void Autocomplete_GotFocus(object sender, RoutedEventArgs e) {
      if (Suggestions != null && Suggestions.Count() > 0) {
        Popup.IsOpen = true;
      }
    }

    private void Autocomplete_LostFocus(object sender, RoutedEventArgs e) {
      Popup.IsOpen = false;
    }

    private void _input_TextChanged(object sender, TextChangedEventArgs e) {
      if (SuggestEngine != null) {
        Suggestions = SuggestEngine.GetSuggestionsFor(Input.Text);
        if (Suggestions.Count() > 0) {
          Popup.IsOpen = true;
        } else {
          Popup.IsOpen = false;
        }
      }
    }

    //private void Autocomplete_MouseDown(object sender, MouseButtonEventArgs e) {
    //  var self = sender as Autocomplete;
    //  var target = FindChild<TextBox>(self, "_input");
    //  //target.Focus();
    //}

    public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject {
      // Confirm parent and childName are valid. 
      if (parent == null) return null;

      T foundChild = null;

      int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
      for (int i = 0; i < childrenCount; i++) {
        var child = VisualTreeHelper.GetChild(parent, i);
        // If the child is not of the request child type child
        T childType = child as T;
        if (childType == null) {
          // recursively drill down the tree
          foundChild = FindChild<T>(child, childName);

          // If the child is found, break so we do not overwrite the found child. 
          if (foundChild != null) break;
        } else if (!string.IsNullOrEmpty(childName)) {
          var frameworkElement = child as FrameworkElement;
          // If the child's name is set for search
          if (frameworkElement != null && frameworkElement.Name == childName) {
            // if the child's name is of the request name
            foundChild = (T)child;
            break;
          }
        } else {
          // child element found.
          foundChild = (T)child;
          break;
        }
      }

      return foundChild;
    }

    private void _lstbox_MouseDown(object sender, MouseButtonEventArgs e) {
      var selected = ListBox.SelectedItem;
      Input.Text = SuggestEngine.ToText(selected);
    }
  }
}
