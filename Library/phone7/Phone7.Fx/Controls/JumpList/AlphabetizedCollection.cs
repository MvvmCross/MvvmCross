using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Phone7.Fx.Controls.JumpList
{
    public class AlphabetizedCollection : ObservableCollection<AlphabetizedItemContainer>
    {
        private readonly string _indexedMember;
        private readonly string _imageMember;

        public AlphabetizedCollection(string indexedMember, string imageMember)
        {
            _indexedMember = indexedMember;
            _imageMember = imageMember;
        }

        private object GetIndexedValue(object item, string indexedMember)
        {
            PropertyInfo propertyInfo = item.GetType().GetProperty(indexedMember);
            if (propertyInfo != null)
                return propertyInfo.GetValue(item, null);
            return null;
        }

        private static char CleanText(char s)
        {
            var e = new[] {
                new { K = 'à', V = 'a' }, 
                new { K = 'â', V = 'a' }, 
                new { K = 'é', V = 'e' },
                new { K = 'è', V = 'e' },
                new { K = 'ê', V = 'e' },
                new { K = 'ï', V = 'i' },
                new { K = 'î', V = 'i' },
                new { K = 'ù', V = 'u' },
                new { K = 'û', V = 'u' },
                new { K = 'ô', V = 'o' },
                new { K = 'ö', V = 'o' }
            };
            foreach (var v in e.Where(v => s.Equals(v.K)))
            {
                return v.V;
            }
            return s;
        }


        public void Add(object item)
        {
            var value = GetIndexedValue(item, _indexedMember);
            if (value == null)
                return;
            char key = CleanText(value.ToString().ToLower().First());

            if (char.IsNumber(key))
                key = '#';

            var image = string.Empty;
            if (!string.IsNullOrEmpty(_imageMember))
                image = GetIndexedValue(item, _imageMember).ToString();

            bool alreadyContainsGroup = !this.Where(c => c.FirstLetter == key).Any();
            if (alreadyContainsGroup)
                base.Add(new AlphabetizedItemContainer { FirstLetter = key, Item = null, ShowGroup = true });
            base.Add(new AlphabetizedItemContainer { FirstLetter = key, Item = item, ShowGroup = false, ImageSource = image });

            base.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

        }

        public void AddRange(IEnumerable items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
    }
}