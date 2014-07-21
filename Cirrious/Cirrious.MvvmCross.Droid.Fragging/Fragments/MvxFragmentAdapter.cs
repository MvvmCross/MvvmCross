using Android.OS;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Fragging.Fragments.EventSource;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Fragging.Fragments
{
    public class MvxFragmentAdapter 
        : MvxBaseFragmentAdapter
    {
        public IMvxFragmentView FragmentView
        {
            get { return Fragment as IMvxFragmentView; }
        }

        public MvxFragmentAdapter(IMvxEventSourceFragment eventSource) 
            : base(eventSource)
        {
        }

        protected override void HandleCreateCalled(object sender, MvxValueEventArgs<Bundle> bundeArgs)
        {
            if (FragmentView.Request != null)
            {
                var loader = Mvx.Resolve<IMvxViewModelLoader>();
                var savedStateConverter = Mvx.Resolve<IMvxSavedStateConverter>();
                var mvxBundle = savedStateConverter.Read(bundeArgs.Value);

                var viewModel = loader.LoadViewModel(FragmentView.Request, mvxBundle);
                FragmentView.ViewModel = viewModel;

                return;
            }

            var cache = Mvx.Resolve<IMvxSingleViewModelCache>();
            var cached = cache.GetAndClear(bundeArgs.Value);
            FragmentView.ViewModel = cached;
        }

        protected override void HandleSaveInstanceStateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            var mvxBundle = FragmentView.CreateSaveStateBundle();
            if (mvxBundle != null)
            {
                IMvxSavedStateConverter converter;
                if (!Mvx.TryResolve(out converter))
                {
                    MvxTrace.Warning("Saved state converter not available - saving state will be hard");
                }
                else
                {
                    converter.Write(bundleArgs.Value, mvxBundle);
                }
            }
            var cache = Mvx.Resolve<IMvxSingleViewModelCache>();
            cache.Cache(FragmentView.ViewModel, bundleArgs.Value);
        }
    }
}