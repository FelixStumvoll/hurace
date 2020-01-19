using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.ViewModels.BaseViewModels;

namespace Hurace.RaceControl.ViewModels.Util
{
    public class FilterableObservableCollection<T> : NotifyPropertyChanged
    {
        private string _searchTerm;

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                Set(ref _searchTerm, value);
                Apply();
            }
        }

        public ObservableCollection<T> ViewItems { get; } = new ObservableCollection<T>();
        public List<T> DataSource { get; } = new List<T>();
        private readonly Func<T, string, bool> _predicate;
        private readonly Func<IEnumerable<T>, IEnumerable<T>> _modifiers;

        public FilterableObservableCollection(Func<T, string, bool> predicate,
            Func<IEnumerable<T>, IEnumerable<T>> modifiers = null)
        {
            _predicate = predicate;
            _modifiers = modifiers;
        }

        public void Apply()
        {
            ViewItems.Clear();
            var loweredSearch = SearchTerm?.ToLower();
            var filtered = DataSource
                .Where(t => SearchTerm.IsNullOrEmpty() || _predicate(t, loweredSearch));
            ViewItems.AddRange(_modifiers?.Invoke(filtered) ?? filtered);
        }

        public void UpdateDataSource(IEnumerable<T> data)
        {
            DataSource.Clear();
            DataSource.AddRange(data);
            Apply();
        }
    }
}