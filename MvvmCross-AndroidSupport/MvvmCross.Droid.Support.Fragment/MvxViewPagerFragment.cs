using System;
using Android.Support.V4.App;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Droid.Support.V4
{
    public class MvxViewPagerFragment
    {
        public MvxViewPagerFragment(string title, Type fragmentType, Type viewModelType, object parameterValuesObject = null)
            : this(title, null, fragmentType, viewModelType, parameterValuesObject)
        {
        }

        public MvxViewPagerFragment(string title, string tag, Type fragmentType, Type viewModelType,
                            object parameterValuesObject = null)
        {
            Title = title;
            Tag = tag ?? title;
            FragmentType = fragmentType;
            ViewModelType = viewModelType;
            ParameterValuesObject = parameterValuesObject;
        }

        public MvxViewPagerFragment(string title, Type fragmentType, IMvxViewModel viewModel, object parameterValuesObject = null)
            : this(title, null, fragmentType, viewModel.GetType(), parameterValuesObject)
        {
            ViewModel = viewModel;
        }

        public MvxViewPagerFragment(string title, string tag, Type fragmentType, IMvxViewModel viewModel, object parameterValuesObject = null)
            : this(title, tag, fragmentType, viewModel.GetType(), parameterValuesObject)
        {
            ViewModel = viewModel;
        }

        public Type FragmentType { get; }

        public object ParameterValuesObject { get; }

        public string Tag { get; }

        public string Title { get; }

        public Type ViewModelType { get; }

        public IMvxViewModel ViewModel { get; }

        public Fragment CachedFragment { get; set; }
    }
}
