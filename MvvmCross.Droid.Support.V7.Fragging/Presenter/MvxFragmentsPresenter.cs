// MvxFragmentsPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Caching;
using System.Threading.Tasks;

namespace MvvmCross.Droid.Support.V7.Fragging.Presenter
{
    public class MvxFragmentsPresenter
        : MvxAndroidViewPresenter
        , IMvxFragmentsPresenter
    {
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";

        private IMvxNavigationSerializer _serializer;

        protected IMvxNavigationSerializer Serializer {
			get {
				if (_serializer != null)
					return _serializer;

				_serializer = Mvx.Resolve<IMvxNavigationSerializer> ();
				return _serializer;
			}
		}

		private readonly Dictionary<Type, Type> _fragments = new Dictionary<Type, Type>();

		public void RegisterFragments(IEnumerable<Type> frags)
		{
			foreach (var item in frags) {
				_fragments[item] = item.GetParentActvityType();
			}
		}

        public override void Show(MvxViewModelRequest request)
        {
			//if (_taskSource != null)
				//TODO: We should wait here for tasks to finish when showing multiple fragments

            var bundle = new Bundle();
            var serializedRequest = Serializer.Serializer.SerializeObject(request);
            bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

			var frag = _fragments.Keys.FirstOrDefault (
				x => x.BaseType.GenericTypeArguments.Contains (request.ViewModelType) 
				|| x.GetViewModelType () == request.ViewModelType);

			Type host;
			if(frag != null && _fragments.TryGetValue(frag, out host))
			{
				var fragmentHost = Activity as IMvxFragmentHost;
				if(fragmentHost != null && host == fragmentHost.GetType())
				{
				    fragmentHost.Show(request, bundle);
				}
				else
				{
					CreateFragmentHostAsync (host).ContinueWith(t => {
						fragmentHost = t.Result;

						if (Activity is IFragmentCacheableActivity) {
							var cache = ((IFragmentCacheableActivity)Activity).FragmentCacheConfiguration;
							if (!cache.HasAnyFragmentsRegisteredToCache) {
								foreach (var item in _fragments.Where(x => x.Value == host)) {

									//TODO: Should only take one GenericTypeArguments of type IMvxViewModel
									var viewModel = item.Key.BaseType?.GenericTypeArguments.FirstOrDefault ()
										?? item.Key.GetViewModelType ();

									cache.RegisterFragmentToCache (viewModel.Name, item.Key, viewModel);
								}
							}
						}

						fragmentHost.Show (request, bundle);
					});

					//var intentFor = new Intent (Activity.ApplicationContext, host);
					//base.Show (intentFor);

					//TODO: Find something to wait for Activity to show, and Show fragment afterwards
					//Show (request);
				}
			}
			else
				base.Show (request);
        }

		private TaskCompletionSource<IMvxFragmentHost> _taskSource;

		public void FragmentHostCreated (IMvxFragmentHost fragmentHost)
		{
			if (_taskSource != null)
			{
				_taskSource.SetResult (fragmentHost);
				_taskSource = null;
			}
		}

		private Task<IMvxFragmentHost> CreateFragmentHostAsync (Type host)
		{
			_taskSource = new TaskCompletionSource<IMvxFragmentHost> ();

			var intentFor = new Intent (Activity.ApplicationContext, host);
			base.Show (intentFor);

			return _taskSource.Task;
		}  

        public override void Close (IMvxViewModel viewModel)
        {
			var frag = _fragments.Keys.FirstOrDefault (
				x => x.BaseType.GenericTypeArguments.Contains (viewModel.GetType()) 
				|| x.GetViewModelType () == viewModel.GetType());
			
			Type host;
			if (frag != null && _fragments.TryGetValue (frag, out host)) {
				var fragmentHost = Activity as IMvxFragmentHost;
				if (fragmentHost != null && host == fragmentHost.GetType ())
				{
				    fragmentHost.Close(viewModel);
				}
			}
			else
				base.Close (viewModel);
        }
    }
}