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
using AutocompleteWPF;

namespace Example {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      TestEngine = new Engine();
      test.SuggestEngine = TestEngine;
    }

    public ICanSuggestCompletion TestEngine {
      get;
      private set;
    }

    public class Engine : ICanSuggestCompletion {
      public IEnumerable<object> GetSuggestionsFor(string input) {
        var list = new List<string>();
        list.Add("Sora");
        list.Add("Sota");
        return list;
      }

      public string ToText(object obj) {
        return "jibril!";
      }
    }

  }
}
