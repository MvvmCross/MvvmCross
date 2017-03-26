namespace $rootnamespace$.Views

open Android.App
open Android.OS
open MvvmCross.Droid.Views

[<Activity(Label = "View for FirstViewModel")>]
type FirstView() =
    inherit MvxActivity()

    override this.OnCreate(bundle: Bundle) =
        base.OnCreate(bundle)
        this.SetContentView(Resource.Layout.FirstView)