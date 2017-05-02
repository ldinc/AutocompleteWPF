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

    #region Text

    public static readonly DependencyProperty TextProperty =
      DependencyProperty.Register("Text", typeof(string), typeof(Autocomplete), new UIPropertyMetadata(string.Empty));

    public string Text {
      get {
        return (string)GetValue(TextProperty);
      }

      set {
        SetValue(TextProperty, value);
      }
    }

    #endregion Text

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

    private ScrollViewer Scroll = null;
    private ListBox ListBox = null;

    private string PrevText = string.Empty;
    private bool ChangedInBackground = false;

    public object Selected = null;

    #endregion InputMappings

    public Autocomplete() {
      InitializeComponent();
      LostFocus += Autocomplete_LostFocus;
      GotFocus += Autocomplete_GotFocus;
      Loaded += Autocomplete_Loaded;
      //ListBox.MouseDown += _lstbox_MouseDown;
      //MouseDown += Autocomplete_MouseDown;
    }

    private void Autocomplete_Loaded(object sender, RoutedEventArgs e) {
      Scroll = (Popup.Child as Border).Child as ScrollViewer;
      ListBox = Scroll.Content as ListBox;
      ListBox.SelectionChanged += ListBox_SelectionChanged;
      ListBox.PreviewMouseDown += ListBox_MouseDown;
    }

    private void ListBox_MouseDown(object sender, MouseButtonEventArgs e) {
      var selected = ListBox.SelectedItem;
      if (selected != null) {
        PrevText = SuggestEngine.ToText(selected);
        Input.Text = PrevText;
        Input.Focus();
        Input.CaretIndex = Input.Text.Length;
        Popup.IsOpen = false;
        Selected = selected;
      }
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (ListBox.IsKeyboardFocusWithin && ListBox.SelectedIndex >= 0) {
        var selected = ListBox.SelectedItem;
        PrevText = SuggestEngine.ToText(selected);
        Input.Text = PrevText;
        Input.Focus();
        Input.CaretIndex = Input.Text.Length;
        Popup.IsOpen = false;
        Selected = selected;
      }
    }

    private void Autocomplete_GotFocus(object sender, RoutedEventArgs e) {
      if (Suggestions != null && Suggestions.Count() > 0 && Selected == null || ChangedInBackground) {
        Popup.IsOpen = true;
        ChangedInBackground = false;
      }
    }

    private void Autocomplete_LostFocus(object sender, RoutedEventArgs e) {
      Popup.IsOpen = false;
    }

    private void _input_TextChanged(object sender, TextChangedEventArgs e) {
      if (SuggestEngine != null) {
        Suggestions = SuggestEngine.GetSuggestionsFor(Input.Text);
        if (PrevText != Input.Text && IsKeyboardFocusWithin) {
          if (Suggestions.Count() > 0) {
            Popup.IsOpen = true;
          } else {
            Popup.IsOpen = false;
          }
          PrevText = Input.Text;
          if (Selected != null) {
            Selected = null;
          }
        } else {
          if (!IsKeyboardFocusWithin) {
            ChangedInBackground = true;
            Selected = null;
          }
        }
      }
    }

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

    private double offset() {
      if (ListBox.SelectedIndex < 0) {
        ListBox.SelectedIndex = 0;
      }
      object selectedItem = ListBox.SelectedItem;
      ListBoxItem selectedListBoxItem = ListBox.ItemContainerGenerator.ContainerFromItem(selectedItem) as ListBoxItem;
      return selectedListBoxItem.ActualHeight;
    }

    private void _input_KeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Down) {
        if (Popup.IsOpen) {
          var index = ListBox.SelectedIndex;
          var d = ListBox.SelectedItemProperty;
          if (index + 1 < ListBox.Items.Count) {
            ListBox.SelectedIndex = index + 1;
            Scroll.ScrollToVerticalOffset(Scroll.ContentVerticalOffset + offset());
          }
        }
        e.Handled = true;
      }
      if (e.Key == Key.Up) {
        if (Popup.IsOpen) {
          var index = ListBox.SelectedIndex;
          if (index > 0) {
            ListBox.SelectedIndex = index - 1;
          }
          Scroll.ScrollToVerticalOffset(Scroll.ContentVerticalOffset - offset());
        }
        e.Handled = true;
      }
      if (e.Key == Key.PageDown) {
        if (Popup.IsOpen) {
          ListBox.SelectedIndex = ListBox.Items.Count - 1;
          Scroll.ScrollToBottom();
        }
        e.Handled = true;
      }
      if (e.Key == Key.PageUp) {
        if (Popup.IsOpen) {
          ListBox.SelectedIndex = 0;
          Scroll.ScrollToTop();
        }
        e.Handled = true;
      }
      if (e.Key == Key.Enter) {
        if (Popup.IsOpen) {
          var selected = ListBox.SelectedItem;
          if (selected != null) {
            PrevText = SuggestEngine.ToText(selected);
            Input.Text = PrevText;
            Input.Focus();
            Input.CaretIndex = Input.Text.Length;
          }
          Popup.IsOpen = false;
          Selected = selected;
        }
        e.Handled = true;
      }
      if (e.Key == Key.Escape) {
        if (Popup.IsOpen) {
          Popup.IsOpen = false;
        }
      }
    }

    private void _input_MouseWheel(object sender, MouseWheelEventArgs e) {
      if (e.Delta > 0 && Popup.IsOpen) {
        Scroll.ScrollToVerticalOffset(Scroll.ContentVerticalOffset + offset());
      } else {
        if (e.Delta < 0 && Popup.IsOpen) {
          Scroll.ScrollToVerticalOffset(Scroll.ContentVerticalOffset - offset());
        }
      }
    }
  }
}
