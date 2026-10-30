// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Hosting;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using NSubstitute;

namespace MvvmCross.UnitTest.Hosting
{
    public class MvxHostBuilderCustomizationTests
    {
        // Minimal concrete builder for testing the base class methods.
        private sealed class TestHostBuilder : MvxHostBuilder
        {
        }

        private static TestHostBuilder CreateTestBuilder() => new TestHostBuilder();

        #region UseNavigationService

        [Test]
        public async Task UseNavigationService_Generic_ResolvesCustomImplementation()
        {
            var builder = CreateTestBuilder();
            builder.UseNavigationService<CustomNavigationService>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxNavigationService>()).IsTypeOf<CustomNavigationService>();
        }

        [Test]
        public async Task UseNavigationService_Factory_ResolvesViaFactory()
        {
            var instance = Substitute.For<IMvxNavigationService>();
            var builder = CreateTestBuilder();
            builder.UseNavigationService(_ => instance);

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxNavigationService>()).IsSameReferenceAs(instance);
        }

        [Test]
        [RequiresUnreferencedCode("Uses AddMvxCore which stores types via reflection.")]
        public async Task UseNavigationService_CalledBeforeStartWith_WinsOverDefault()
        {
            var builder = CreateTestBuilder();

            // Custom registration BEFORE StartWith (which calls AddMvxCore / TryAddSingleton)
            builder.UseNavigationService<CustomNavigationService>();
            builder.StartWith<StubViewModel>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxNavigationService>()).IsTypeOf<CustomNavigationService>();
        }

        [Test]
        [RequiresUnreferencedCode("Uses AddMvxCore which stores types via reflection.")]
        public async Task UseNavigationService_CalledAfterStartWith_WinsOverDefault()
        {
            var builder = CreateTestBuilder();

            // Custom registration AFTER StartWith (which calls AddMvxCore / TryAddSingleton)
            builder.StartWith<StubViewModel>();
            builder.UseNavigationService<CustomNavigationService>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxNavigationService>()).IsTypeOf<CustomNavigationService>();
        }

        [Test]
        public async Task UseNavigationService_CalledTwice_LastOneWins()
        {
            var builder = CreateTestBuilder();
            builder.UseNavigationService<CustomNavigationService>();
            builder.UseNavigationService<AnotherCustomNavigationService>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxNavigationService>()).IsTypeOf<AnotherCustomNavigationService>();
        }

        [Test]
        public void UseNavigationService_NullFactory_Throws()
        {
            var builder = CreateTestBuilder();
            Assert.Throws<ArgumentNullException>(() => builder.UseNavigationService<IMvxNavigationService>(null!));
        }

        [Test]
        public async Task UseNavigationService_ReturnsBuilderForChaining()
        {
            var builder = CreateTestBuilder();
            var result = builder.UseNavigationService<CustomNavigationService>();
            await Assert.That(result).IsSameReferenceAs(builder);
        }

        #endregion

        #region UsePresenter

        [Test]
        public async Task UsePresenter_Generic_ResolvesCustomImplementation()
        {
            var builder = CreateTestBuilder();
            builder.UsePresenter<CustomViewPresenter>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxViewPresenter>()).IsTypeOf<CustomViewPresenter>();
        }

        [Test]
        public async Task UsePresenter_Factory_ResolvesViaFactory()
        {
            var instance = Substitute.For<IMvxViewPresenter>();
            var builder = CreateTestBuilder();
            builder.UsePresenter(_ => instance);

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxViewPresenter>()).IsSameReferenceAs(instance);
        }

        [Test]
        public void UsePresenter_NullFactory_Throws()
        {
            var builder = CreateTestBuilder();
            Assert.Throws<ArgumentNullException>(() => builder.UsePresenter<IMvxViewPresenter>(null!));
        }

        [Test]
        public async Task UsePresenter_ReturnsBuilderForChaining()
        {
            var builder = CreateTestBuilder();
            var result = builder.UsePresenter<CustomViewPresenter>();
            await Assert.That(result).IsSameReferenceAs(builder);
        }

        #endregion

        #region UseAppStart

        [Test]
        public async Task UseAppStart_Generic_ResolvesCustomImplementation()
        {
            var builder = CreateTestBuilder();
            builder.UseAppStart<CustomAppStart>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxAppStart>()).IsTypeOf<CustomAppStart>();
        }

        [Test]
        public async Task UseAppStart_Factory_ResolvesViaFactory()
        {
            var instance = Substitute.For<IMvxAppStart>();
            var builder = CreateTestBuilder();
            builder.UseAppStart(_ => instance);

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxAppStart>()).IsSameReferenceAs(instance);
        }

        [Test]
        [RequiresUnreferencedCode("Uses AddMvxCore which stores types via reflection.")]
        public async Task UseAppStart_CalledBeforeStartWith_WinsOverDefault()
        {
            var builder = CreateTestBuilder();
            builder.UseAppStart<CustomAppStart>();
            builder.StartWith<StubViewModel>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxAppStart>()).IsTypeOf<CustomAppStart>();
        }

        [Test]
        [RequiresUnreferencedCode("Uses AddMvxCore which stores types via reflection.")]
        public async Task UseAppStart_CalledAfterStartWith_WinsOverDefault()
        {
            var builder = CreateTestBuilder();
            builder.StartWith<StubViewModel>();
            builder.UseAppStart<CustomAppStart>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxAppStart>()).IsTypeOf<CustomAppStart>();
        }

        [Test]
        public async Task UseAppStart_CalledTwice_LastOneWins()
        {
            var builder = CreateTestBuilder();
            builder.UseAppStart<CustomAppStart>();
            builder.UseAppStart<AnotherCustomAppStart>();

            var provider = builder.Services.BuildServiceProvider();

            await Assert.That(provider.GetRequiredService<IMvxAppStart>()).IsTypeOf<AnotherCustomAppStart>();
        }

        [Test]
        public void UseAppStart_NullFactory_Throws()
        {
            var builder = CreateTestBuilder();
            Assert.Throws<ArgumentNullException>(() => builder.UseAppStart<IMvxAppStart>(null!));
        }

        [Test]
        public async Task UseAppStart_ReturnsBuilderForChaining()
        {
            var builder = CreateTestBuilder();
            var result = builder.UseAppStart<CustomAppStart>();
            await Assert.That(result).IsSameReferenceAs(builder);
        }

        #endregion

        #region Default logging

        [Test]
        public async Task AddMvxCore_WithoutExplicitAddLogging_ILoggerFactory_ResolvesSuccessfully()
        {
            var host = new CustomHostBuilder()
                .StartWith<StubViewModel>()
                .Build();

            var loggerFactory = host.Services.GetService<ILoggerFactory>();
            await Assert.That(loggerFactory).IsNotNull();
        }

        [Test]
        public async Task AddMvxCore_WithoutExplicitAddLogging_ILoggerT_ResolvesSuccessfully()
        {
            var host = new CustomHostBuilder()
                .StartWith<StubViewModel>()
                .Build();

            var logger = host.Services.GetService<ILogger<StubViewModel>>();
            await Assert.That(logger).IsNotNull();
        }

        [Test]
        public async Task AddMvxCore_UserAddLogging_DoesNotBreakFramework()
        {
            var host = new CustomHostBuilder()
                .StartWith<StubViewModel>()
                .ConfigureServices(s => s.AddLogging(b => b.SetMinimumLevel(LogLevel.Debug)))
                .Build();

            // ILoggerFactory must still be resolvable after user customisation.
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            await Assert.That(loggerFactory).IsNotNull();
        }

        #endregion

        #region UseViewDispatcher

        [Test]
        public async Task UseViewDispatcher_Generic_ResolvesCustomImplementation()
        {
            var host = new CustomHostBuilder()
                .UseViewDispatcher<CustomViewDispatcher>()
                .StartWith<StubViewModel>()
                .Build();

            var dispatcher = host.Services.GetRequiredService<IMvxViewDispatcher>();
            await Assert.That(dispatcher).IsTypeOf<CustomViewDispatcher>();
        }

        [Test]
        public async Task UseViewDispatcher_Factory_ResolvesViaFactory()
        {
            var expected = new CustomViewDispatcher();
            var host = new CustomHostBuilder()
                .UseViewDispatcher(_ => expected)
                .StartWith<StubViewModel>()
                .Build();

            var dispatcher = host.Services.GetRequiredService<IMvxViewDispatcher>();
            await Assert.That(dispatcher).IsSameReferenceAs(expected);
        }

        [Test]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("")]
        public async Task UseViewDispatcher_CalledBeforeStartWith_WinsOverDefault()
        {
            var host = new CustomHostBuilder()
                .UseViewDispatcher<CustomViewDispatcher>()
                .StartWith<StubViewModel>()
                .Build();

            await Assert.That(host.Services.GetRequiredService<IMvxViewDispatcher>()).IsTypeOf<CustomViewDispatcher>();
        }

        [Test]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("")]
        public async Task UseViewDispatcher_CalledAfterStartWith_WinsOverDefault()
        {
            var host = new CustomHostBuilder()
                .StartWith<StubViewModel>()
                .UseViewDispatcher<CustomViewDispatcher>()
                .Build();

            await Assert.That(host.Services.GetRequiredService<IMvxViewDispatcher>()).IsTypeOf<CustomViewDispatcher>();
        }

        [Test]
        public async Task UseViewDispatcher_CalledTwice_LastOneWins()
        {
            var host = new CustomHostBuilder()
                .UseViewDispatcher<CustomViewDispatcher>()
                .UseViewDispatcher<AnotherCustomViewDispatcher>()
                .StartWith<StubViewModel>()
                .Build();

            await Assert.That(host.Services.GetRequiredService<IMvxViewDispatcher>()).IsTypeOf<AnotherCustomViewDispatcher>();
        }

        [Test]
        public void UseViewDispatcher_NullFactory_Throws()
        {
            var builder = new CustomHostBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                builder.UseViewDispatcher((Func<IServiceProvider, CustomViewDispatcher>)null!));
        }

        [Test]
        public async Task UseViewDispatcher_ReturnsBuilderForChaining()
        {
            var builder = new CustomHostBuilder();
            var result = builder.UseViewDispatcher<CustomViewDispatcher>();
            await Assert.That(result).IsSameReferenceAs(builder);
        }

        #endregion

        #region MvxHost subclassing

        [Test]
        public async Task MvxHost_CanBeSubclassed_AndCreatedWithPublicConstructor()
        {
            var services = new ServiceCollection().BuildServiceProvider();
            var host = new CustomMvxHost(services);
            await Assert.That(host).IsNotNull();
        }

        [Test]
        public async Task CreateHost_Override_ReturnsCustomSubclass()
        {
            var builder = new CustomHostBuilder();
            var host = builder.Build();
            await Assert.That(host).IsTypeOf<CustomMvxHost>();
        }

        #endregion

        #region Stub types

        private sealed class StubViewModel : MvxViewModel
        {
        }

        private sealed class CustomNavigationService : IMvxNavigationService
        {
            public event EventHandler<IMvxNavigateEventArgs>? WillNavigate;
            public event EventHandler<IMvxNavigateEventArgs>? DidNavigate;
            public event EventHandler<IMvxNavigateEventArgs>? WillClose;
            public event EventHandler<IMvxNavigateEventArgs>? DidClose;
            public event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;
            public event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;

            public void LoadRoutes(IEnumerable<Assembly> assemblies) { }
            public Task<bool> CanNavigate(string path) => Task.FromResult(false);
            public Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel => Task.FromResult(false);
            public Task<bool> CanNavigate(Type viewModelType) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Close(IMvxViewModel viewModel, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate(IMvxViewModel viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate(string path, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>(string path, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel<TParameter> where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel<TParameter> where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate(IMvxViewModel viewModel, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);
        }

        // Distinct type (not a subclass of CustomNavigationService) for "last one wins" test.
        private sealed class AnotherCustomNavigationService : IMvxNavigationService
        {
            public event EventHandler<IMvxNavigateEventArgs>? WillNavigate;
            public event EventHandler<IMvxNavigateEventArgs>? DidNavigate;
            public event EventHandler<IMvxNavigateEventArgs>? WillClose;
            public event EventHandler<IMvxNavigateEventArgs>? DidClose;
            public event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;
            public event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;

            public void LoadRoutes(IEnumerable<Assembly> assemblies) { }
            public Task<bool> CanNavigate(string path) => Task.FromResult(false);
            public Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel => Task.FromResult(false);
            public Task<bool> CanNavigate(Type viewModelType) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Close(IMvxViewModel viewModel, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate(IMvxViewModel viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate(string path, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>(string path, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel<TParameter> where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel<TParameter> where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate(IMvxViewModel viewModel, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TParameter : notnull => Task.FromResult(false);
        }

        private sealed class CustomViewPresenter : IMvxViewPresenter
        {
            [RequiresUnreferencedCode("")]
            public Task<bool> Show(MvxViewModelRequest request) => Task.FromResult(false);

            [RequiresUnreferencedCode("")]
            public Task<bool> ChangePresentation(MvxPresentationHint hint) => Task.FromResult(false);

            public void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action)
                where THint : MvxPresentationHint
            {
            }

            [RequiresUnreferencedCode("")]
            public Task<bool> Close(IMvxViewModel viewModel) => Task.FromResult(false);
        }

        private sealed class CustomAppStart : IMvxAppStart
        {
            public bool IsStarted => false;
            [RequiresUnreferencedCode("")]
            public void Start(object? hint = null) { }
            [RequiresUnreferencedCode("")]
            public Task StartAsync(object? hint = null) => Task.CompletedTask;
            public void ResetStart() { }
        }

        private sealed class AnotherCustomAppStart : IMvxAppStart
        {
            public bool IsStarted => false;
            [RequiresUnreferencedCode("")]
            public void Start(object? hint = null) { }
            [RequiresUnreferencedCode("")]
            public Task StartAsync(object? hint = null) => Task.CompletedTask;
            public void ResetStart() { }
        }

        private sealed class CustomMvxHost : MvxHost
        {
            public CustomMvxHost(IServiceProvider services) : base(services) { }
        }

        private sealed class CustomHostBuilder : MvxHostBuilder
        {
            protected override MvxHost CreateHost(IServiceProvider serviceProvider)
                => new CustomMvxHost(serviceProvider);
        }

        private sealed class CustomViewDispatcher : IMvxViewDispatcher
        {
            public bool IsOnMainThread => true;
            public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true) => Task.CompletedTask;
            public Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true) => Task.CompletedTask;
            public bool RequestMainThreadAction(Action action, bool maskExceptions = true) => true;

            [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("")]
            public Task<bool> ShowViewModel(MvxViewModelRequest request) => Task.FromResult(false);

            [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("")]
            public Task<bool> ChangePresentation(MvxPresentationHint hint) => Task.FromResult(false);
        }

        private sealed class AnotherCustomViewDispatcher : IMvxViewDispatcher
        {
            public bool IsOnMainThread => true;
            public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true) => Task.CompletedTask;
            public Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true) => Task.CompletedTask;
            public bool RequestMainThreadAction(Action action, bool maskExceptions = true) => true;

            [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("")]
            public Task<bool> ShowViewModel(MvxViewModelRequest request) => Task.FromResult(false);

            [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("")]
            public Task<bool> ChangePresentation(MvxPresentationHint hint) => Task.FromResult(false);
        }

        #endregion
    }
}
