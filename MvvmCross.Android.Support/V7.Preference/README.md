# V7.Preference Support

In `Setup.cs` override `FillTargetFactories` to call `MvxPreferenceSetupHelper.FillTargetFactories`.

For example:
```csharp
protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
{
    base.FillTargetFactories(registry);

    MvxPreferenceSetupHelper.FillTargetFactories(registry);
}
```

Then one can bind to classes like `EditTextPreference`, `Preference`, etc.

As an example given a `preferences.xml` like the following:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<PreferenceScreen xmlns:android="http://schemas.android.com/apk/res/android">
  <EditTextPreference android:title="Server Address"
                      android:key="pref_server"
                      android:summary="URL of the Server"/>
  <Preference android:title="Reset Settings"
              android:key="pref_reset"
              android:summary="Restores all settings to default"/>
</PreferenceScreen>
```

One can bind to the ViewModel's `ServerAddress` property or `ResetSettings` command:
```csharp
public class SettingsViewFragment : MvxPreferenceFragmentCompat<SettingsViewModel>
{   
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        // Since the view is created in PreferenceFragmentCompat's OnCreateView
        // we don't use BindingInflate like a typical MvxFragment.
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        var serverPreference = (EditTextPreference)this.FindPreference("pref_server");
        var resetSettingsPreference = this.FindPreference("pref_reset");

        var bindingSet = this.CreateBindingSet<SettingsViewFragment, SettingsViewModel>();
        bindingSet.Bind(serverPreference)
            .For(v => v.Text)
            .To(vm => vm.ServerAddress);
        bindingSet.Bind(resetSettingsPreference)
            .For("PreferenceClick")
            .To(vm => vm.ResetSettings);
        bindingSet.Apply();

        return view;
    }

    public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
    {
        this.AddPreferencesFromResource(Resource.Xml.preferences);
    }
}
```
