using System;
using System.Collections.Generic;
using Example.Core.ViewModels;
using Example.Droid.Fragments;
using MvvmCross.Droid.Support.V7.Fragging.Caching;

namespace Example.Droid.Activities.Caching
{
    internal class MainActivityFragmentCacheInfoFactory : MvxCachedFragmentInfoFactory
    {
        private static readonly Dictionary<string, CustomFragmentInfo> MyFragmentsInfo = new Dictionary
            <string, CustomFragmentInfo>
        {
            {
                typeof (MenuViewModel).ToString(),
                new CustomFragmentInfo(typeof (MenuViewModel).Name, typeof (MenuFragment), typeof (MenuViewModel))
            },
            {
                typeof (HomeViewModel).ToString(),
                new CustomFragmentInfo(typeof (HomeViewModel).Name, typeof (HomeFragment), typeof (HomeViewModel),
                    isRoot: true)
            },
            {
                typeof (ExampleViewPagerViewModel).ToString(),
                new CustomFragmentInfo(typeof (ExampleViewPagerViewModel).Name, typeof (ExampleViewPagerFragment),
                    typeof (ExampleViewPagerViewModel), isRoot: true)
            },
            {
                typeof (ExampleViewPagerStateViewModel).ToString(),
                new CustomFragmentInfo(typeof (ExampleViewPagerStateViewModel).Name, typeof (ExampleViewPagerStateFragment),
                    typeof (ExampleViewPagerStateViewModel), isRoot: true, cacheFragment: false)
            },
            {
                typeof (ExampleRecyclerViewModel).ToString(),
                new CustomFragmentInfo(typeof (ExampleRecyclerViewModel).Name, typeof (ExampleRecyclerViewFragment),
                    typeof (ExampleRecyclerViewModel), isRoot: true)
            },
            {
                typeof (SettingsViewModel).ToString(),
                new CustomFragmentInfo(typeof (SettingsViewModel).Name, typeof (SettingsFragment),
                    typeof (SettingsViewModel), isRoot: true)
            }
        };

        public Dictionary<string, CustomFragmentInfo> GetFragmentsRegistrationData()
        {
            return MyFragmentsInfo;
        }

        public override IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true,
            bool addToBackstack = false)
        {
            var viewModelTypeString = viewModelType.ToString();
            if (!MyFragmentsInfo.ContainsKey(viewModelTypeString))
                return base.CreateFragmentInfo(tag, fragmentType, viewModelType, cacheFragment, addToBackstack);

            var fragInfo = MyFragmentsInfo[viewModelTypeString];
            return fragInfo;
        }

        public override SerializableMvxCachedFragmentInfo GetSerializableFragmentInfo(
            IMvxCachedFragmentInfo objectToSerialize)
        {
            var baseSerializableCachedFragmentInfo = base.GetSerializableFragmentInfo(objectToSerialize);
            var customFragmentInfo = objectToSerialize as CustomFragmentInfo;

            return new SerializableCustomFragmentInfo(baseSerializableCachedFragmentInfo)
            {
                IsRoot = customFragmentInfo?.IsRoot ?? false
            };
        }

        public override IMvxCachedFragmentInfo ConvertSerializableFragmentInfo(
            SerializableMvxCachedFragmentInfo fromSerializableMvxCachedFragmentInfo)
        {
            var serializableCustomFragmentInfo = fromSerializableMvxCachedFragmentInfo as SerializableCustomFragmentInfo;
            var baseCachedFragmentInfo = base.ConvertSerializableFragmentInfo(fromSerializableMvxCachedFragmentInfo);

            return new CustomFragmentInfo(baseCachedFragmentInfo.Tag, baseCachedFragmentInfo.FragmentType,
                baseCachedFragmentInfo.ViewModelType, baseCachedFragmentInfo.AddToBackStack,
                serializableCustomFragmentInfo?.IsRoot ?? false)
            {
                ContentId = baseCachedFragmentInfo.ContentId,
                CachedFragment = baseCachedFragmentInfo.CachedFragment
            };
        }

        internal class SerializableCustomFragmentInfo : SerializableMvxCachedFragmentInfo
        {
            public SerializableCustomFragmentInfo()
            {
                
            }

            public SerializableCustomFragmentInfo(SerializableMvxCachedFragmentInfo baseFragmentInfo)
            {
                AddToBackStack = baseFragmentInfo.AddToBackStack;
                ContentId = baseFragmentInfo.ContentId;
                FragmentType = baseFragmentInfo.FragmentType;
                Tag = baseFragmentInfo.Tag;
                ViewModelType = baseFragmentInfo.ViewModelType;
            }

            public bool IsRoot { get; set; }
        }
    }
}