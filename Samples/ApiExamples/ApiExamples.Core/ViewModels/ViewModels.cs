using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ApiExamples.Core.ViewModels.Helpers;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.ViewModels;

namespace ApiExamples.Core.ViewModels
{
    public class StripConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Replace((string)parameter, string.Empty);
        }
    }

    public class FirstViewModel
        : MvxViewModel
    {
        public FirstViewModel(IAllTestsService service)
        {
            Tests = service.All;
        }

        private IList<Type> _tests;
        public IList<Type> Tests
        {
            get { return _tests; }
            set { _tests = value; RaisePropertyChanged(() => Tests); }
        }

        public ICommand GotoTestCommand
        {
            get { return new MvxCommand<Type>(type => ShowViewModel(type)); }
        }
    }

    public abstract class TestViewModel
        : MvxViewModel
    {
        public ICommand NextCommand
        {
            get
            {
                return new MvxCommand(() =>
                    {
                        var all = Mvx.Resolve<IAllTestsService>();
                        var next = all.NextViewModelType(this);
                        if (next == null)
                            Close(this);
                        else
                            ShowViewModel(next);
                    });
            }
        }
    }

    public class DateTimeViewModel
        : TestViewModel
    {
        private DateTime _property = DateTime.Now;
        public DateTime Property
        {
            get { return _property; }
            set { _property = value; RaisePropertyChanged(() => Property); }
        }
    }

    public class TimeViewModel
        : TestViewModel
    {
        private TimeSpan _property = DateTime.Now.TimeOfDay;
        public TimeSpan Property
        {
            get { return _property; }
            set { _property = value; RaisePropertyChanged(() => Property); }
        }
    }

    public class SpinnerViewModel
        : TestViewModel
    {
        public class Thing
        {
            public Thing(string caption)
            {
                Caption = caption;
            }

            public string Caption { get; private set; }

            public override string ToString()
            {
                return Caption;
            }

            public override bool Equals(object obj)
            {
                var rhs = obj as Thing;
                if (rhs == null)
                    return false;
                return rhs.Caption == Caption;
            }

            public override int GetHashCode()
            {
                if (Caption == null)
                    return 0;
                return Caption.GetHashCode();
            }
        }
        private List<Thing> _items = new List<Thing>()
            {
                new Thing("One"),
                new Thing("Two"),
                new Thing("Three"),
                new Thing("Four"),
            };
        public List<Thing> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        private Thing _selectedItem = new Thing("Three");
        public Thing SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged(() => SelectedItem); }
        }
    }

    public abstract class BaseListTestViewModel
        : TestViewModel
    {
        private ObservableCollection<string> _items = new ObservableCollection<string>()
            {
                "One", "Two", "Three"
            };
        public ObservableCollection<string> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        private string _selected;
        public string Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged(() => Selected); }
        }

        public ICommand AddCommand
        {
            get { return new MvxCommand(() => Items.Add(Guid.NewGuid().ToString("N"))); }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (Items.Count == 0)
                        return;
                    Items.RemoveAt(0);
                });
            }
        }


    }
    public class ListViewModel : BaseListTestViewModel
    {
        private int i = 0;
        public ICommand Hello
        {
            get { return new MvxCommand(() => Mvx.Trace("Hello " + ++i));}
        }
    }

    public class LinearLayoutViewModel : BaseListTestViewModel
    { }

    public class FrameViewModel : BaseListTestViewModel
    { }

    public class RelativeViewModel : BaseListTestViewModel
    { }

    public class ObservableCollectionViewModel : TestViewModel
    {
        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        public ICommand ReplaceAllCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Items = new ObservableCollection<string>()
                            {
                                "One " + Guid.NewGuid().ToString(), 
                                "Two " + Guid.NewGuid().ToString(), 
                                "Three " + Guid.NewGuid().ToString() 
                            };
                });
            }
        }

        public ICommand ReplaceEachCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (Items == null)
                    {
                        Items = new ObservableCollection<string>();
                        Items.Add("1 " + Guid.NewGuid().ToString());
                        Items.Add("2 " + Guid.NewGuid().ToString());
                        Items.Add("3 " + Guid.NewGuid().ToString());
                        return;
                    }

                    Items[0] = "1 " + Guid.NewGuid().ToString();
                    Items[1] = "2 " + Guid.NewGuid().ToString();
                    Items[2] = "3 " + Guid.NewGuid().ToString();
                });
            }
        }

        public ICommand MakeNullCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Items = null;
                });
            }
        }
    }

    public class ObservableDictionaryViewModel : TestViewModel
    {
        private ObservableDictionary<string, string> _items;
        public ObservableDictionary<string, string> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        public ICommand ReplaceAllCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Items = new ObservableDictionary<string, string>()
                            {
                                {"One", "One " + Guid.NewGuid().ToString() }, 
                                {"Two","Two " + Guid.NewGuid().ToString() },
                                {"Three","Three " + Guid.NewGuid().ToString() }
                            };
                });
            }
        }

        public ICommand ReplaceEachCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (Items == null)
                    {
                        Items = new ObservableDictionary<string, string>() {
                                {"One", "One " + Guid.NewGuid().ToString() }, 
                                {"Two","Two " + Guid.NewGuid().ToString() },
                                {"Three","Three " + Guid.NewGuid().ToString() }
                            };
                        return;
                    }

                    Items["One"] = "1 " + Guid.NewGuid().ToString();
                    Items["Two"] = "2 " + Guid.NewGuid().ToString();
                    Items["Three"] = "3 " + Guid.NewGuid().ToString();
                });
            }
        }

        public ICommand MakeNullCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Items = null;
                });
            }
        }
    }

    public class WithErrorsViewModel : TestViewModel
    {
        private ObservableDictionary<string, string> _errors = new ObservableDictionary<string, string>();
        public ObservableDictionary<string, string> Errors
        {
            get { return _errors; }
            set { _errors = value; RaisePropertyChanged(() => Errors); }
        }

        private string _email = "Enter Email Here";
        public string Email
        {
            get { return _email; }
            set { _email = value; RaisePropertyChanged(() => Email); Validate(); }
        }

        private bool _acceptTerms;
        public bool AcceptTerms
        {
            get { return _acceptTerms; }
            set { _acceptTerms = value; RaisePropertyChanged(() => AcceptTerms); Validate(); }
        }

        public WithErrorsViewModel()
        {
            Validate();
        }

        private void Validate()
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(_email);
            UpdateError(!match.Success, "Email", "Please enter a valid Email address");
            UpdateError(!_acceptTerms, "AcceptTerms", "Please accept the terms");
        }

        private void UpdateError(bool isInError, string propertyName, string errorMessage)
        {
            if (isInError)
            {
                Errors[propertyName] = errorMessage;
            }
            else
            {
                if (Errors.ContainsKey(propertyName))
                    Errors.Remove(propertyName);
            }
        }
    }


    public class TextViewModel : TestViewModel
    {
        private string _stringProperty = "Hello";
        public string StringProperty
        {
            get { return _stringProperty; }
            set { _stringProperty = value; RaisePropertyChanged(() => StringProperty); }
        }

        private double _doubleProperty = 42.12;
        public double DoubleProperty
        {
            get { return _doubleProperty; }
            set { _doubleProperty = value; RaisePropertyChanged(() => DoubleProperty); }
        }
    }

    public class SeekViewModel : TestViewModel
    {
        private double _seekProperty = 12;
        public double SeekProperty
        {
            get { return _seekProperty; }
            set { _seekProperty = value; RaisePropertyChanged(() => SeekProperty); }
        }
    }

    public class ContainsSubViewModel : TestViewModel
    {
        public class PersonViewModel : MvxNotifyPropertyChanged
        {
            private string _firstName;
            public string FirstName
            {
                get { return _firstName; }
                set { _firstName = value; RaisePropertyChanged(() => FirstName); }
            }

            private string _lastName;
            public string LastName
            {
                get { return _lastName; }
                set { _lastName = value; RaisePropertyChanged(() => LastName); }
            }
        }

        private PersonViewModel _firstPerson = new PersonViewModel()
        {
            FirstName = "Fred",
            LastName = "Flintstone"
        };
        public PersonViewModel FirstPerson
        {
            get { return _firstPerson; }
            set { _firstPerson = value; RaisePropertyChanged(() => FirstPerson); }
        }

        private PersonViewModel _secondPerson = new PersonViewModel()
        {
            FirstName = "Barney",
            LastName = "Rubble"
        };
        public PersonViewModel SecondPerson
        {
            get { return _secondPerson; }
            set { _secondPerson = value; RaisePropertyChanged(() => SecondPerson); }
        }
    }

    public class PlusTenValueConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)value) + 10;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return int.Parse((string)value) - 10;
        }
    }

    public class ConvertThisViewModel : TestViewModel
    {
        private int _value = 21;
        public int Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(() => Value); }
        }
    }

    public class IfViewModel : TestViewModel
    {
        private int _testVal1 = 0;
        public int TestVal1 
        {   
            get { return _testVal1; }
            set { _testVal1 = value; RaisePropertyChanged(() => TestVal1); }
        }

        private int _testVal2 = 10;
        public int TestVal2 
        {   
            get { return _testVal2; }
            set { _testVal2 = value; RaisePropertyChanged(() => TestVal2); }
        }
    }

    public class MathsViewModel : TestViewModel
    {
        private int _testVal1 = 10;
        public int TestVal1
        {
            get { return _testVal1; }
            set { _testVal1 = value; RaisePropertyChanged(() => TestVal1); }
        }

        private int _testVal2 = 2;
        public int TestVal2
        {
            get { return _testVal2; }
            set { _testVal2 = value; RaisePropertyChanged(() => TestVal2); }
        }
    }

    public class RadioGroupViewModel
    : TestViewModel
    {
        public class Thing
        {
            public Thing(string caption)
            {
                Caption = caption;
            }

            public string Caption { get; private set; }

            public override string ToString()
            {
                return Caption;
            }

            public override bool Equals(object obj)
            {
                var rhs = obj as Thing;
                if (rhs == null)
                    return false;
                return rhs.Caption == Caption;
            }

            public override int GetHashCode()
            {
                if (Caption == null)
                    return 0;
                return Caption.GetHashCode();
            }
        }
        private List<Thing> _items = new List<Thing>()
            {
                new Thing("One"),
                new Thing("Two"),
                new Thing("Three"),
                new Thing("Four"),
            };
        public List<Thing> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        private Thing _selectedItem = new Thing("Three");
        public Thing SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged(() => SelectedItem); }
        }
    }

}
