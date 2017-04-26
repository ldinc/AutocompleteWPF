using System.Collections.Generic;

namespace AutocompleteWPF {
  public interface ICanSuggestCompletion {
    //int Count();
    IEnumerable<object> GetSuggestionsFor(string input);
    string ToText(object obj);
  }
}
