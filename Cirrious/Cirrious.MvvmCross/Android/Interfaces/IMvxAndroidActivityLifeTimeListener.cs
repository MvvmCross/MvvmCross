using Android.App;

namespace Cirrious.MvvmCross.Android.Interfaces
{
    public interface IMvxAndroidActivityLifetimeListener
    {
        void OnCreate(Activity activity);
        void OnStart(Activity activity);
        void OnRestart(Activity activity);
        void OnResume(Activity activity);
        void OnPause(Activity activity);
        void OnStop(Activity activity);
        void OnDestroy(Activity activity);
    }
}