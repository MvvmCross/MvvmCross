using Microsoft.CodeAnalysis;
using MvvmCross.CodeAnalysis.Analyzers;
using MvvmCross.CodeAnalysis.CodeFixes;
using MvvmCross.CodeAnalysis.Core;
using NUnit.Framework;

namespace MvvmCross.CodeAnalysis.Test
{
    [TestFixture]
    public class MvxMessengerSubscriptionDoesntStoreTokenInVariableAnalyzerTests : CodeFixVerifier<MvxMessengerSubscriptionDoesntStoreTokenInVariableAnalyzer, MvxMessengerSubscriptionDoesntStoreTokenInVariableCodeFix>
    {
        private const string Test = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public FirstViewModel(IMvxMessenger messenger)
        {
            messenger.Subscribe<AndroidApp.Core.Messages.LocationMessage>(OnLocationMessage);
        }

        private static void OnLocationMessage(AndroidApp.Core.Messages.LocationMessage message)
        {
        }
    }
}";

        private const string Expected = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        private readonly MvxSubscriptionToken _token;
        public FirstViewModel(IMvxMessenger messenger)
        {
            _token = messenger.Subscribe<AndroidApp.Core.Messages.LocationMessage>(OnLocationMessage);
        }

        private static void OnLocationMessage(AndroidApp.Core.Messages.LocationMessage message)
        {
        }
    }
}";

        private const string TestWithNoUsing = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MvvmCross.Core.ViewModels;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        public FirstViewModel(MvvmCross.Plugins.Messenger.IMvxMessenger messenger)
        {
            messenger.Subscribe<AndroidApp.Core.Messages.LocationMessage>(OnLocationMessage);
        }

        private static void OnLocationMessage(AndroidApp.Core.Messages.LocationMessage message)
        {
        }
    }
}";

        private const string ExpectedWithNoUsing = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace AndroidApp.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        private readonly MvxSubscriptionToken _token;

        public FirstViewModel(IMvxMessenger messenger)
        {
            messenger.Subscribe<AndroidApp.Core.Messages.LocationMessage>(OnLocationMessage);
        }

        private static void OnLocationMessage(AndroidApp.Core.Messages.LocationMessage message)
        {
        }
    }
}";

        [Test]
        public void MvxMessengerSubscriptionDoesntStoreTokenInVariableDiagnostic()
        {
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = DiagnosticIds.MvxMessengerSubscriptionDoesntStoreTokenInVariableId,
                Message = "You need to store the token returned from 'messenger.Subscribe<AndroidApp.Core.Messages.LocationMessage>(OnLocationMessage)'.",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 15, 13)
                    }
            };

            var project = new[]
            {
                new MvxTestFileSource(Test, MvxProjType.Core),
                new MvxTestFileSource(MvxMessenger, MvxProjType.Core),
                new MvxTestFileSource(MvxMessage, MvxProjType.Core),
                new MvxTestFileSource(LocationMessage, MvxProjType.Core)
            };

            VerifyCSharpDiagnostic(project, expectedDiagnostic);
        }

        [Test]
        public void MvxMessengerSubscriptionDoesntStoreTokenInVariableAnalyzerShouldFixTheCode()
        {
            var project = new[]
            {
                new MvxTestFileSource(MvxMessenger, MvxProjType.Core),
                new MvxTestFileSource(MvxMessage, MvxProjType.Core),
                new MvxTestFileSource(LocationMessage, MvxProjType.Core),
                new MvxTestFileSource(MvxSubscriptionToken, MvxProjType.Core)
            };

            VerifyCSharpFix(project, Test, MvxProjType.Core, Expected);
        }

        [Test]
        public void MvxMessengerSubscriptionDoesntStoreTokenInVariableAnalyzerShouldFixTheCodeEvenWithoutProperUsingDirectives()
        {
            var project = new[]
            {
                new MvxTestFileSource(MvxMessenger, MvxProjType.Core),
                new MvxTestFileSource(MvxMessage, MvxProjType.Core),
                new MvxTestFileSource(LocationMessage, MvxProjType.Core),
                new MvxTestFileSource(MvxSubscriptionToken, MvxProjType.Core)
            };

            VerifyCSharpFix(project, TestWithNoUsing, MvxProjType.Core, ExpectedWithNoUsing);
        }

        #region Work around to missing IMvxMessengerReferences


        private const string MvxSubscriptionToken = @"
using System;

namespace MvvmCross.Plugins.Messenger
{
    public sealed class MvxSubscriptionToken
        : IDisposable
    {
        public Guid Id { get; private set; }
#pragma warning disable 414 // 414 is that this private field is only set, not used
        private readonly object[] _dependentObjects;
#pragma warning restore 414
        private readonly Action _disposeMe;

        public MvxSubscriptionToken(Guid id, Action disposeMe, params object[] dependentObjects)
        {
            Id = id;
            _disposeMe = disposeMe;
            _dependentObjects = dependentObjects;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _disposeMe();
            }
        }
    }
}";

        private const string MvxMessenger = @"using System;
using System.Collections.Generic;

namespace MvvmCross.Plugins.Messenger
{
    public interface IMvxMessenger
    {
        MvxSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, MvxReference reference = MvxReference.Weak, string tag = null)
            where TMessage : MvxMessage;

        MvxSubscriptionToken SubscribeOnMainThread<TMessage>(Action<TMessage> deliveryAction, MvxReference reference = MvxReference.Weak, string tag = null)
             where TMessage : MvxMessage;

        MvxSubscriptionToken SubscribeOnThreadPoolThread<TMessage>(Action<TMessage> deliveryAction, MvxReference reference = MvxReference.Weak, string tag = null)
             where TMessage : MvxMessage;

        void Unsubscribe<TMessage>(MvxSubscriptionToken mvxSubscriptionId)
            where TMessage : MvxMessage;

        bool HasSubscriptionsFor<TMessage>()
             where TMessage : MvxMessage;

        int CountSubscriptionsFor<TMessage>()
             where TMessage : MvxMessage;

        bool HasSubscriptionsForTag<TMessage>(string tag)
             where TMessage : MvxMessage;

        int CountSubscriptionsForTag<TMessage>(string tag)
             where TMessage : MvxMessage;

        IList<string> GetSubscriptionTagsFor<TMessage>()
             where TMessage : MvxMessage;

        void Publish<TMessage>(TMessage message) where TMessage : MvxMessage;

        void Publish(MvxMessage message);

        void Publish(MvxMessage message, Type messageType);

        void RequestPurge(Type messageType);

        void RequestPurgeAll();
    }
}";

        private const string MvxMessage = @"using System;

namespace MvvmCross.Plugins.Messenger
{
    public abstract class MvxMessage
    {
        public object Sender { get; private set; }

        protected MvxMessage(object sender)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            Sender = sender;
        }
    }
}";

        private const string LocationMessage = @"namespace AndroidApp.Core.Messages
{
    using MvvmCross.Plugins.Messenger;

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
}";
    }

    #endregion
}