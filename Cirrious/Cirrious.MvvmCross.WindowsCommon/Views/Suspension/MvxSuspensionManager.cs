// MvxSuspensionManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cirrious.MvvmCross.WindowsCommon.Platform;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Suspension
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
        private const string SessionStateFilename = "_mvxSessionState.xml";

        private Dictionary<string, object> _sessionState = new Dictionary<string, object>();
        private readonly List<Type> _knownTypes = new List<Type>() { typeof(Dictionary<string, string>) }; //Bundles are saved as string-string dicts, serializer needs to be warned of this type or will throw.

        /// <summary>
        /// Provides access to global session state for the current session.  This state is
        /// serialized by <see cref="SaveAsync"/> and restored by
        /// <see cref="RestoreAsync"/>, so values must be serializable by
        /// <see cref="DataContractSerializer"/> and should be as compact as possible.  Strings
        /// and other self-contained data types are strongly recommended.
        /// </summary>
        public Dictionary<string, object> SessionState
        {
            get { return _sessionState; }
        }

        /// <summary>
        /// List of custom types provided to the <see cref="DataContractSerializer"/> when
        /// reading and writing session state.  Initially empty, additional types may be
        /// added to customize the serialization process.
        /// </summary>
        public List<Type> KnownTypes
        {
            get { return _knownTypes; }
        }

        /// <summary>
        /// Save the current <see cref="SessionState"/>.  Any <see cref="Frame"/> instances
        /// registered with <see cref="RegisterFrame"/> will also preserve their current
        /// navigation stack, which in turn gives their active <see cref="Page"/> an opportunity
        /// to save its state.
        /// </summary>
        /// <returns>An asynchronous task that reflects when session state has been saved.</returns>
        public async Task SaveAsync()
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
                var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                serializer.WriteObject(sessionData, _sessionState);

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
        public async Task RestoreAsync()
        {
            _sessionState = new Dictionary<String, Object>();

            try
            {
                // Get the input stream for the SessionState file
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(SessionStateFilename);
                using (var inStream = await file.OpenSequentialReadAsync())
                {
                    // Deserialize the Session State
                    var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                    _sessionState = (Dictionary<string, object>)serializer.ReadObject(inStream.AsStreamForRead());
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

        private DependencyProperty MvxFrameSessionStateKeyProperty =
            DependencyProperty.RegisterAttached("_MvxFrameSessionStateKey", typeof(String), typeof(MvxSuspensionManager), null);
        private DependencyProperty MvxFrameSessionStateProperty =
            DependencyProperty.RegisterAttached("_MvxFrameSessionState", typeof(Dictionary<String, Object>), typeof(MvxSuspensionManager), null);
        private List<WeakReference<IMvxWindowsFrame>> _registeredFrames = new List<WeakReference<IMvxWindowsFrame>>();

        /// <summary>
        /// Registers a <see cref="Frame"/> instance to allow its navigation history to be saved to
        /// and restored from <see cref="SessionState"/>.  Frames should be registered once
        /// immediately after creation if they will participate in session state management.  Upon
        /// registration if state has already been restored for the specified key
        /// the navigation history will immediately be restored.  Subsequent invocations of
        /// <see cref="RestoreAsync"/> will also restore navigation history.
        /// </summary>
        /// <param name="frame">An instance whose navigation history should be managed by
        /// <see cref="MvxSuspensionManager"/></param>
        /// <param name="sessionStateKey">A unique key into <see cref="SessionState"/> used to
        /// store navigation-related information.</param>
        public void RegisterFrame(IMvxWindowsFrame frame, String sessionStateKey)
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

        /// <summary>
        /// Disassociates a <see cref="Frame"/> previously registered by <see cref="RegisterFrame"/>
        /// from <see cref="SessionState"/>.  Any navigation state previously captured will be
        /// removed.
        /// </summary>
        /// <param name="frame">An instance whose navigation history should no longer be
        /// managed.</param>
        public void UnregisterFrame(IMvxWindowsFrame frame)
        {
            // Remove session state and remove the frame from the list of frames whose navigation
            // state will be saved (along with any weak references that are no longer reachable)
            SessionState.Remove((String)frame.GetValue(MvxFrameSessionStateKeyProperty));
            _registeredFrames.RemoveAll((weakFrameReference) =>
            {
                IMvxWindowsFrame testFrame;
                return !weakFrameReference.TryGetTarget(out testFrame) || testFrame == frame;
            });
        }

        /// <summary>
        /// Provides storage for session state associated with the specified <see cref="Frame"/>.
        /// Frames that have been previously registered with <see cref="RegisterFrame"/> have
        /// their session state saved and restored automatically as a part of the global
        /// <see cref="SessionState"/>.  Frames that are not registered have transient state
        /// that can still be useful when restoring pages that have been discarded from the
        /// navigation cache.
        /// </summary>
        /// <remarks>Apps may choose to rely on <see cref="LayoutAwarePage"/> to manage
        /// page-specific state instead of working with frame session state directly.</remarks>
        /// <param name="frame">The instance for which session state is desired.</param>
        /// <returns>A collection of state subject to the same serialization mechanism as
        /// <see cref="SessionState"/>.</returns>
        public Dictionary<String, Object> SessionStateForFrame(IMvxWindowsFrame frame)
        {
            var frameState = (Dictionary<String, Object>)frame.GetValue(MvxFrameSessionStateProperty);

            if (frameState == null)
            {
                var frameSessionKey = (String)frame.GetValue(MvxFrameSessionStateKeyProperty);
                if (frameSessionKey != null)
                {
                    // Registered frames reflect the corresponding session state
                    if (!_sessionState.ContainsKey(frameSessionKey))
                    {
                        _sessionState[frameSessionKey] = new Dictionary<String, Object>();
                    }
                    frameState = (Dictionary<String, Object>)_sessionState[frameSessionKey];
                }
                else
                {
                    // Frames that aren't registered have transient state
                    frameState = new Dictionary<String, Object>();
                }
                frame.SetValue(MvxFrameSessionStateProperty, frameState);
            }
            return frameState;
        }

        private void RestoreFrameNavigationState(IMvxWindowsFrame frame)
        {
            var frameState = SessionStateForFrame(frame);
            if (frameState.ContainsKey("Navigation"))
            {
                frame.SetNavigationState((String)frameState["Navigation"]);
            }
        }

        private void SaveFrameNavigationState(IMvxWindowsFrame frame)
        {
            var frameState = SessionStateForFrame(frame);
            frameState["Navigation"] = frame.GetNavigationState();
        }
    }
}
