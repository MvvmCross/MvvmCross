using System;
using System.Reflection;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Helpers
{
    /// <summary>Provides methods for event management. </summary>
    public class EventUtilities
    {
#if !LEGACY

        /// <summary>Registers an event on the given target object. </summary>
        /// <param name="target">The target object. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="callback">The callback. </param>
        /// <returns>The registration token to deregister the event. </returns>
        public static object RegisterEvent(object target, string eventName, Action<object, object> callback)
        {
            var callbackMethodInfo = callback.GetMethodInfo();
            var eventInfo = target.GetType().GetRuntimeEvent(eventName);
            var callbackDelegate = callbackMethodInfo.CreateDelegate(eventInfo.EventHandlerType, callback.Target);
            return eventInfo.AddMethod.Invoke(target, new object[] { callbackDelegate });
        }

        /// <summary>Registers a static event on the given target object. </summary>
        /// <param name="type">The target type. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="callback">The callback. </param>
        /// <returns>The registration token to deregister the event. </returns>
        public static object RegisterStaticEvent(Type type, string eventName, Action<object, object> callback)
        {
            var callbackMethodInfo = callback.GetMethodInfo();
            var eventInfo = type.GetRuntimeEvent(eventName);
            var callbackDelegate = callbackMethodInfo.CreateDelegate(eventInfo.EventHandlerType, callback.Target);
            return eventInfo.AddMethod.Invoke(null, new object[] { callbackDelegate });
        }

        /// <summary>Deregisters an event from the target object. </summary>
        /// <param name="target">The target object. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="token">The registration token. </param>
        public static void DeregisterEvent(object target, string eventName, object token)
        {
            var eventInfo = target.GetType().GetRuntimeEvent(eventName);
            eventInfo.RemoveMethod.Invoke(target, new object[] { token });
        }

        /// <summary>Deregisters a static event from the target type. </summary>
        /// <param name="type">The target type. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="token">The registration token. </param>
        public static void DeregisterStaticEvent(Type type, string eventName, object token)
        {
            var eventInfo = type.GetRuntimeEvent(eventName);
            eventInfo.RemoveMethod.Invoke(null, new object[] { token });
        }

#else

        /// <summary>Registers an event on the given target object. </summary>
        /// <param name="target">The target object. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="callback">The callback. </param>
        /// <returns>The registration token to deregister the event. </returns>
        public static object RegisterEvent(object target, string eventName, Action<object, object> callback)
        {
            var callbackMethodInfo = callback.Method;
            var eventInfo = target.GetType().GetEvent(eventName);
            var callbackDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, callback.Target, callbackMethodInfo);
            return eventInfo.GetAddMethod().Invoke(target, new object[] { callbackDelegate });
        }

        /// <summary>Registers a static event on the given target object. </summary>
        /// <param name="type">The target type. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="callback">The callback. </param>
        /// <returns>The registration token to deregister the event. </returns>
        public static object RegisterStaticEvent(Type type, string eventName, Action<object, object> callback)
        {
            var callbackMethodInfo = callback.Method;
            var eventInfo = type.GetEvent(eventName);
            var callbackDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, callback.Target, callbackMethodInfo);
            return eventInfo.GetAddMethod().Invoke(null, new object[] { callbackDelegate });
        }

        /// <summary>Deregisters an event from the target object. </summary>
        /// <param name="target">The target object. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="token">The registration token. </param>
        public static void DeregisterEvent(object target, string eventName, object token)
        {
            var eventInfo = target.GetType().GetEvent(eventName);
            eventInfo.GetRemoveMethod().Invoke(target, new object[] { token });
        }

        /// <summary>Deregisters a static event from the target type. </summary>
        /// <param name="type">The target type. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="token">The registration token. </param>
        public static void DeregisterStaticEvent(Type type, string eventName, object token)
        {
            var eventInfo = type.GetEvent(eventName);
            eventInfo.GetRemoveMethod().Invoke(null, new object[] { token });
        }

#endif
    }
}
