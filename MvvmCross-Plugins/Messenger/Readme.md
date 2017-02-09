### Messenger

The MvvmCross `Messenger` plugin provides an Event aggregation Messenger which is biased towards using Weak references for event subscription.

The `IMvxMessenger` API includes:

- publishing methods:
  - `Publish`
- subscription methods:
  - `Subscribe`, `SubscribeOnMainThread`, `SubscribeOnThreadPoolThread`, `Unsubscribe`
- subscription state observation methods:
  - `HasSubscriptionsFor`, `HasSubscriptionsForTag`, `CountSubscriptionsFor`, `CountSubscriptionsForTag`, `GetSubscriptionTagsFor`
- clearup methods:
  - `RequestPurge`, `RequestPurgeAll`  



The basic use of the `Messenger` is:

- define one or more Message classes for communication between components. These should inherit from `MvxMessage` - e.g.:

	    public class LocationMessage
	        : MvxMessage
	    {
	        public LocationMessage(object sender, double lat, double lng) 
	            : base(sender)
	        {
	            Lng = lng;
	            Lat = lat;
	        }
	
	        public double Lat { get; private set; }
	        public double Lng { get; private set; }
	    }

- define the classes which will create and send these Messages - e.g. a `LocationService` might create and send `LocationMessage`s using

       var message = new LocationMessage(
           this,
           location.Coordinates.Latitude,
           location.Coordinates.Longitude
           );

       _messenger.Publish(message);

- define the classes which will subscribe to and receive these messages. Each of these classes must call one of the `Subscribe` methods on the `IMvxMessenger` and **must store the returned token**. For example part of a ViewModel receivin `LocationMessage`s might look like:

	    public class LocationViewModel 
			: MvxViewModel
	    {
	        private readonly MvxSubscriptionToken _token;
	
	        public LocationViewModel(IMvxMessenger messenger)
	        {
	            _token = messenger.Subscribe<LocationMessage>(OnLocationMessage);
	        }
	
	        private void OnLocationMessage(LocationMessage locationMessage)
	        {
	            Lat = locationMessage.Lat;
	            Lng = locationMessage.Lng;
	        }
	        
	        // remainder of ViewModel
	    }

The three different options for subscribing for messages differ only in terms of which thread messages will be passed back on:

- `Subscribe` - messages will be passed directly on the `Publish` thread. These subscriptions have the lowest processing overhead - messages will always be received synchronously whenever they are published. You should use this type of subscription if you already know which type of thread the Publish will be called on and if you have a good understanding on the resource and UI usage of your message handler.
- `SubscribeOnMainThread` - any message published on a background thread will be marshalled to the main UI thread.  This type of subscription is ideal if your message handler needs to perform some resource-unintensive task which involves interacting with the UI.
- `SubscribeOnThreadPoolThread` - messages will always be queued for thread pool processing. This always involves an asynchonous post - even if the message is published on an existing ThreadPool thread. This type of subscription is ideal if your message handler needs to perform some resource-intensive task as it won't block the UI, nor the message publisher.

All subscription methods have two additional parameters:

- `MvxReference reference = MvxReference.Weak` - specify `MvxReference.Strong` if you would prefer to use `Strong` references - in this case, the Messenger will keep a strong reference to the callback method and Garbage Collection will not be able to remove the subscription.
- `string tag = null` - an optional `tag` which allows code to inspect what Message listeners are currently listening for - see 'observe the current subscription status' below.

Subscriptions can be cancelled at any time using the `Unsubscribe` method on the `IMvxMessenger` or by calling `Dispose()` on the subscription token.

However, in many cases, `Unsubscribe`/`Dispose` is never called. Instead listeners rely on the `WeakReference` implementation of the  `MvxSubscriptionToken` to clear up the subscription when objects go out of scope and Garbage Collection occurs.

This GC-based unsubscription will occur whenever the subscription token returned from `Subscribe` is Garbage Collected - so if the token is **not** stored, then unsubscription may occur immediately - e.g. in this method

     public void MayNotEverReceiveAMessage()
     {
         var token = _messenger.Subscribe<MyMessage>((message) => {
             Mvx.Trace("Message received!");
         });
         // token goes out of scope now 
         // - so will be garbage collected *at some point*
         // - so trace may never get called
     }

For any code wishing to observe the current subscription status on any message type (including subscriptions that have been requested with a named string `tag`) then this can be done:

- using the `HasSubscriptionsFor` and `CountSubscriptionsFor` methods
- by subscribing for `MvxSubscriberChangeMessage` messages - the Messenger itself publishes these `MvxSubscriberChangeMessage` messages whenever subscriptions are made, are removed or have expired.

	    public class MvxSubscriberChangeMessage : MvxMessage
	    {
	        public Type MessageType { get; private set; }
	        public int SubscriberCount { get; private set; }
	
	        public MvxSubscriberChangeMessage(object sender, Type messageType, int countSubscribers = 0) 
	            : base(sender)
	        {
	            SubscriberCount = countSubscribers;
	            MessageType = messageType;
	        }
	    }

These mechanisms allow you to author singleton services which can adapt their resource requirements according to the current needs of the app. 

For example, suppose you have a service which tracks stock prices using calls to a web service. Individual clients might subscribe to Messages from this service for individual stock codes. The stock service can track when subscribers are present for each stock code and can then adjust which network calls it makes.