// MvxAsyncAndroidSetupSingleton.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

using System;
using System.Threading;

using Android.Content;

using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.IoC;
using System.Threading.Tasks;

namespace MvvmCross.Droid.Platform
{
	public class MvxAsyncAndroidSetupSingleton
		: MvxSingleton<MvxAndroidSetupSingleton>
	{
		private MvxAsyncAndroidSetup _setup;
		private bool _initialized;
		private bool _initializationStarted;
		private IMvxAndroidSplashScreenActivity _currentSplashScreen;

		private SemaphoreSlim _ensureInitializedSemaphore = new SemaphoreSlim(1);

		public virtual async Task EnsureInitializedAsync()
		{
			await _ensureInitializedSemaphore.WaitAsync().ConfigureAwait(false);

			try
			{
				if (_initialized) return;

				_initializationStarted = true;
				await _setup.InitializeAsync().ConfigureAwait(false);
				_initialized = true;

				if (_currentSplashScreen != null)
				{
					Mvx.Warning(
						"Current splash screen not null during direct initialization - " + 
						"not sure this should ever happen!");
					_currentSplashScreen.InitializationComplete();
				}
			}
			finally
			{
				_ensureInitializedSemaphore.Release();
			}
		}

		public virtual void RemoveSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
		{
			_currentSplashScreen = null;
		}

		public virtual async Task InitializeFromSplashScreenAsync(IMvxAndroidSplashScreenActivity splashScreen)
		{
			await _ensureInitializedSemaphore.WaitAsync().ConfigureAwait(false);

			try
			{
				_currentSplashScreen = splashScreen;

				if (_initializationStarted && _initialized)
				{
					_currentSplashScreen?.InitializationComplete();
					return;
				}

				_initializationStarted = true;

				await _setup.InitializeAsync().ConfigureAwait(false);

				_initialized = true;
				_currentSplashScreen?.InitializationComplete();
			}
			finally 
			{
				_ensureInitializedSemaphore.Release();
			}
		}

		public static MvxAndroidSetupSingleton EnsureSingletonAvailable(Context applicationContext)
		{
			if (Instance != null)
				return Instance;

			var instance = new MvxAsyncAndroidSetupSingleton();
			instance.CreateSetup(applicationContext);
			return Instance;
		}

		protected MvxAsyncAndroidSetupSingleton()
		{
		}

		protected virtual void CreateSetup(Context applicationContext)
		{
			var setupType = FindSetupType();
			if (setupType == null)
				throw new MvxException("Could not find a Setup class for application");

			try
			{
				_setup = (MvxAsyncAndroidSetup)Activator.CreateInstance(setupType, applicationContext);
			}
			catch (Exception exception)
			{
				throw exception.MvxWrap("Failed to create instance of {0}", setupType.FullName);
			}
		}

		protected virtual Type FindSetupType()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.ExceptionSafeGetTypes())
				{
					if (type.Name != "Setup") continue;
					if (!typeof(MvxAsyncAndroidSetup).IsAssignableFrom(type)) continue;

					return type;
				}
			}

			return default(Type);
		}

		protected override void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				_currentSplashScreen = null;
			}
			base.Dispose(isDisposing);
		}
	}
}
