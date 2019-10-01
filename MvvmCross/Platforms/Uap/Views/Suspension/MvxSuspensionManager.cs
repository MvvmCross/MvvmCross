// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MvvmCross.Platforms.Uap.Views.Suspension
{
    /// <summary>
    /// MvxSuspensionManager captures global session state to simplify process lifetime management
    /// for an application.  Note that session state will be automatically cleared under a variety
    /// of conditions and should only be used to store information that would be convenient to
    /// carry across sessions, but that should be discarded when an application crashes or is
    /// upgraded.
    /// </summary>
    public class MvxSuspensionManager : IMvxSuspensionManager
    {
        protected const string SessionStateFilename = "_mvxSessionState.xml";

        /// <summary>
        /// Provides access to global session state for the current session.  This state is
        /// serialized by <see cref="SaveAsync"/> and restored by
        /// <see cref="RestoreAsync"/>, so values must be serializable by
        /// <see cref="DataContractSerializer"/> and should be as compact as possible.  Strings
        /// and other self-contained data types are strongly recommended.
        /// </summary>
        public Dictionary<string, object> SessionState { get; protected set; } = new Dictionary<string, object>();

        /// <summary>
        /// List of custom types provided to the <see cref="DataContractSerializer"/> when
        /// reading and writing session state.  Initially empty, additional types may be
        /// added to customize the serialization process.
        /// </summary>
        public List<Type> KnownTypes { get; } = new List<Type>() { typeof(Dictionary<string, string>) };

        /// <summary>
        /// Save the current <see cref="SessionState"/>.  Any <see cref="Frame"/> instances
        /// registered with <see cref="RegisterFrame"/> will also preserve their current
        /// navigation stack, which in turn gives their active <see cref="Page"/> an opportunity
        /// to save its state.
        /// </summary>
        /// <returns>An asynchronous task that reflects when session state has been saved.</returns>
        public virtual async Task SaveAsync()
        {
            try
            {
                // Save the navigation state for all registered frames
                foreach (var weakFrameReference in _registeredFrames)
                {
                    IMvxWindowsFrame frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        SaveFrameNavigationState(frame);
                    }
                }

                // Serialize the session state synchronously to avoid asynchronous access to shared
                // state
                var sessionData = new MemoryStream();
                var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), KnownTypes);
                serializer.WriteObject(sessionData, SessionState);

                // Get an output stream for the SessionState file and write the state asynchronously
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(SessionStateFilename, CreationCollisionOption.ReplaceExisting);
                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                throw new MvxSuspensionManagerException(e);
            }
        }

        /// <summary>
        /// Restores previously saved <see cref="SessionState"/>.  Any <see cref="Frame"/> instances
        /// registered with <see cref="RegisterFrame"/> will also restore their prior navigation
        /// state, which in turn gives their active <see cref="Page"/> an opportunity restore its
        /// state.
        /// </summary>
        /// <returns>An asynchronous task that reflects when session state has been read.  The
        /// content of <see cref="SessionState"/> should not be relied upon until this task
        /// completes.</returns>
        public virtual async Task RestoreAsync()
        {
            SessionState = new Dictionary<string, object>();

            try
            {
                // Get the input stream for the SessionState file
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(SessionStateFilename);
                using (var inStream = await file.OpenSequentialReadAsync())
                {
                    // Deserialize the Session State
                    var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), KnownTypes);
                    SessionState = (Dictionary<string, object>)serializer.ReadObject(inStream.AsStreamForRead());
                }

                // Restore any registered frames to their saved state
                foreach (var weakFrameReference in _registeredFrames)
                {
                    IMvxWindowsFrame frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        frame.ClearValue(MvxFrameSessionStateProperty);
                        RestoreFrameNavigationState(frame);
                    }
                }
            }
            catch (Exception e)
            {
                throw new MvxSuspensionManagerException(e);
            }
        }

        protected readonly DependencyProperty MvxFrameSessionStateKeyProperty =
            DependencyProperty.RegisterAttached("_MvxFrameSessionStateKey", typeof(string), typeof(MvxSuspensionManager), null);
        protected readonly DependencyProperty MvxFrameSessionStateProperty =
            DependencyProperty.RegisterAttached("_MvxFrameSessionState",
                typeof(Dictionary<string, object>), typeof(MvxSuspensionManager), null);
        protected readonly List<WeakReference<IMvxWindowsFrame>> _registeredFrames = new List<WeakReference<IMvxWindowsFrame>>();

        public virtual void RegisterFrame(IMvxWindowsFrame frame, string sessionStateKey)
        {
            if (frame.GetValue(MvxFrameSessionStateKeyProperty) != null)
            {
                throw new InvalidOperationException("Frames can only be registered to one session state key");
            }

            if (frame.GetValue(MvxFrameSessionStateProperty) != null)
            {
                throw new InvalidOperationException("Frames must be either be registered before accessing frame session state, or not registered at all");
            }

            // Use a dependency property to associate the session key with a frame, and keep a list of frames whose
            // navigation state should be managed
            frame.SetValue(MvxFrameSessionStateKeyProperty, sessionStateKey);
            _registeredFrames.Add(new WeakReference<IMvxWindowsFrame>(frame));

            // Check to see if navigation state can be restored
            RestoreFrameNavigationState(frame);
        }

        public virtual void UnregisterFrame(IMvxWindowsFrame frame)
        {
            // Remove session state and remove the frame from the list of frames whose navigation
            // state will be saved (along with any weak references that are no longer reachable)
            SessionState.Remove((string)frame.GetValue(MvxFrameSessionStateKeyProperty));
            _registeredFrames.RemoveAll((weakFrameReference) =>
            {
                IMvxWindowsFrame testFrame;
                return !weakFrameReference.TryGetTarget(out testFrame) || testFrame == frame;
            });
        }

        public virtual Dictionary<string, object> SessionStateForFrame(IMvxWindowsFrame frame)
        {
            var frameState = (Dictionary<string, object>)frame.GetValue(MvxFrameSessionStateProperty);

            if (frameState == null)
            {
                var frameSessionKey = (string)frame.GetValue(MvxFrameSessionStateKeyProperty);
                if (frameSessionKey != null)
                {
                    // Registered frames reflect the corresponding session state
                    if (!SessionState.ContainsKey(frameSessionKey))
                    {
                        SessionState[frameSessionKey] = new Dictionary<string, object>();
                    }
                    frameState = (Dictionary<string, object>)SessionState[frameSessionKey];
                }
                else
                {
                    // Frames that aren't registered have transient state
                    frameState = new Dictionary<string, object>();
                }
                frame.SetValue(MvxFrameSessionStateProperty, frameState);
            }
            return frameState;
        }

        protected virtual void RestoreFrameNavigationState(IMvxWindowsFrame frame)
        {
            var frameState = SessionStateForFrame(frame);
            if (frameState.ContainsKey("Navigation"))
            {
                frame.SetNavigationState((string)frameState["Navigation"]);
            }
        }

        protected virtual void SaveFrameNavigationState(IMvxWindowsFrame frame)
        {
            var frameState = SessionStateForFrame(frame);
            frameState["Navigation"] = frame.GetNavigationState();
        }
    }
}
