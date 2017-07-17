---
layout: documentation
title: Location
category: Plugins
---
The `Location` plugin provides access to GeoLocation (typically GPS) functionality via the API:

```c#
public interface IMvxLocationWatcher
{
    void Start(
        MvxLocationOptions options, 
        Action<MvxGeoLocation> success, 
        Action<MvxLocationError> error);
    void Stop();
    bool Started { get; }
    
    MvxGeoLocation CurrentLocation { get; }
    MvxGeoLocation LastSeenLocation { get; }
    
    event EventHandler<MvxValueEventArgs<MvxLocationPermission>> OnPermissionChanged;
}
```

The `Location` plugin is implemented on all platforms (even Wpf).

Because of the `Action` based nature of the `IMvxLocationWatcher` API, it's generally best **not** to use this interface directly inside ViewModels, but instead to use the API in a singleton service which can then send Messages to your ViewModels.

An example implementation of such a service is:

```c#
public class LocationService
    : ILocationService
{
    private readonly IMvxLocationWatcher _watcher;
    private readonly IMvxMessenger _messenger;

    public LocationService(IMvxLocationWatcher watcher, IMvxMessenger messenger)
    {
        _watcher = watcher;
        _messenger = messenger;
        _watcher.Start(new MvxLocationOptions(), OnLocation, OnError);
    }

    private void OnLocation(MvxGeoLocation location)
    {
        var message = new LocationMessage(this,
                                            location.Coordinates.Latitude,
                                            location.Coordinates.Longitude);

        _messenger.Publish(message);
    }

    private void OnError(MvxLocationError error)
    {
        Mvx.Error("Seen location error {0}", error.Code);
    }
}
```

For a good walk-through of using the location plugin, including using it in tandem with the MvvmCross messenger, see both N=8 and N=9 in N+1 videos of MvvmCross - [N+1 videos](https://github.com/slodge/MvvmCross/wiki/N-1-Videos-Of-MvvmCross)


Notes:

- the `MvxLocationOptions` object passed into the Start method provides a number of options like `EnableHighAccuracy` - not all of these options are well implemented on all platforms. For iOS8 and later, these options include the type of location permission request used (see https://github.com/MvvmCross/MvvmCross/pull/789)
- the default implementation is a good 'general' module if you just need 'location information' in your app. If your app requires more - e.g. control over time and distance tracking, geo-fencing, etc - then consider building your own plugin (or injecting your own service from the UI project on each platform). The source code for the Location plugin should provide you with a good starting place for this.
- it's not unusual for Android developers to hit issues with location detection on different phones and on different Android - check StackOverflow and Issues for questions and answers - e.g. https://github.com/slodge/MvvmCross/issues/360
- some platforms (especially Android) insist on the Location Watcher being started/stopped on the UI thread
- the MvvmCross coordinates object - MvxCoordinates - does not currently come with any built-in maths operations. Algorithms for some common coordinate operations can be found on (for example) http://slodge.blogspot.co.uk/2012/04/calculating-distance-between-latlng.html.


### Known issue

1. Android plugin does not track location in case `MvxLocationOptions` does not match current device GPS mode. For example `MvxLocationOptions.Coarse` works only with **Battery saving** mode and does not work for **High accuracy** and **Device only**. As a solution use Fused plugin: `MvvmCross.Plugins.Location.Fused.Droid`

### Fused Location (Android only)

There is a new **preferred** way to access location in Android which is called `FusedLocationApi`: http://developer.android.com/intl/ru/training/location/index.html 

There are few benefits of using it (over the old approach):

- `Coarse` option means **Low power** mode
- location still tracked when device's GPS mode chagned
- the plugin is more power efficient in general

NOTE: the plugin requires GooglePlayServices (which is not installed by default on emulators)

This plugin is not compatible with default `MvvmCross.Plugins.Location.Droid` plugin. Ensure only one is used in Android project. 
