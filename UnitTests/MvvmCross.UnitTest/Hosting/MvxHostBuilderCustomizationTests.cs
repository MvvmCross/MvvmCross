// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Hosting;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using NSubstitute;
using Xunit;

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

        [Fact]
        public void UseNavigationService_Generic_ResolvesCustomImplementation()
        {
            var builder = CreateTestBuilder();
            builder.UseNavigationService<CustomNavigationService>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<CustomNavigationService>(provider.GetRequiredService<IMvxNavigationService>());
        }

        [Fact]
        public void UseNavigationService_Factory_ResolvesViaFactory()
        {
            var instance = Substitute.For<IMvxNavigationService>();
            var builder = CreateTestBuilder();
            builder.UseNavigationService(_ => instance);

            var provider = builder.Services.BuildServiceProvider();

            Assert.Same(instance, provider.GetRequiredService<IMvxNavigationService>());
        }

        [Fact]
        [RequiresUnreferencedCode("Uses AddMvvmCross which stores types via reflection.")]
        public void UseNavigationService_CalledBeforeStartWith_WinsOverDefault()
        {
            var builder = CreateTestBuilder();

            // Custom registration BEFORE StartWith (which calls AddMvvmCross / TryAddSingleton)
            builder.UseNavigationService<CustomNavigationService>();
            builder.StartWith<StubViewModel>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<CustomNavigationService>(provider.GetRequiredService<IMvxNavigationService>());
        }

        [Fact]
        [RequiresUnreferencedCode("Uses AddMvvmCross which stores types via reflection.")]
        public void UseNavigationService_CalledAfterStartWith_WinsOverDefault()
        {
            var builder = CreateTestBuilder();

            // Custom registration AFTER StartWith (which calls AddMvvmCross / TryAddSingleton)
            builder.StartWith<StubViewModel>();
            builder.UseNavigationService<CustomNavigationService>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<CustomNavigationService>(provider.GetRequiredService<IMvxNavigationService>());
        }

        [Fact]
        public void UseNavigationService_CalledTwice_LastOneWins()
        {
            var builder = CreateTestBuilder();
            builder.UseNavigationService<CustomNavigationService>();
            builder.UseNavigationService<AnotherCustomNavigationService>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<AnotherCustomNavigationService>(provider.GetRequiredService<IMvxNavigationService>());
        }

        [Fact]
        public void UseNavigationService_NullFactory_Throws()
        {
            var builder = CreateTestBuilder();
            Assert.Throws<ArgumentNullException>(
                () => builder.UseNavigationService<IMvxNavigationService>(null!));
        }

        [Fact]
        public void UseNavigationService_ReturnsBuilderForChaining()
        {
            var builder = CreateTestBuilder();
            var result = builder.UseNavigationService<CustomNavigationService>();
            Assert.Same(builder, result);
        }

        #endregion

        #region UsePresenter

        [Fact]
        public void UsePresenter_Generic_ResolvesCustomImplementation()
        {
            var builder = CreateTestBuilder();
            builder.UsePresenter<CustomViewPresenter>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<CustomViewPresenter>(provider.GetRequiredService<IMvxViewPresenter>());
        }

        [Fact]
        public void UsePresenter_Factory_ResolvesViaFactory()
        {
            var instance = Substitute.For<IMvxViewPresenter>();
            var builder = CreateTestBuilder();
            builder.UsePresenter(_ => instance);

            var provider = builder.Services.BuildServiceProvider();

            Assert.Same(instance, provider.GetRequiredService<IMvxViewPresenter>());
        }

        [Fact]
        public void UsePresenter_NullFactory_Throws()
        {
            var builder = CreateTestBuilder();
            Assert.Throws<ArgumentNullException>(
                () => builder.UsePresenter<IMvxViewPresenter>(null!));
        }

        [Fact]
        public void UsePresenter_ReturnsBuilderForChaining()
        {
            var builder = CreateTestBuilder();
            var result = builder.UsePresenter<CustomViewPresenter>();
            Assert.Same(builder, result);
        }

        #endregion

        #region UseAppStart

        [Fact]
        public void UseAppStart_Generic_ResolvesCustomImplementation()
        {
            var builder = CreateTestBuilder();
            builder.UseAppStart<CustomAppStart>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<CustomAppStart>(provider.GetRequiredService<IMvxAppStart>());
        }

        [Fact]
        public void UseAppStart_Factory_ResolvesViaFactory()
        {
            var instance = Substitute.For<IMvxAppStart>();
            var builder = CreateTestBuilder();
            builder.UseAppStart(_ => instance);

            var provider = builder.Services.BuildServiceProvider();

            Assert.Same(instance, provider.GetRequiredService<IMvxAppStart>());
        }

        [Fact]
        [RequiresUnreferencedCode("Uses AddMvvmCross which stores types via reflection.")]
        public void UseAppStart_CalledBeforeStartWith_WinsOverDefault()
        {
            var builder = CreateTestBuilder();
            builder.UseAppStart<CustomAppStart>();
            builder.StartWith<StubViewModel>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<CustomAppStart>(provider.GetRequiredService<IMvxAppStart>());
        }

        [Fact]
        [RequiresUnreferencedCode("Uses AddMvvmCross which stores types via reflection.")]
        public void UseAppStart_CalledAfterStartWith_WinsOverDefault()
        {
            var builder = CreateTestBuilder();
            builder.StartWith<StubViewModel>();
            builder.UseAppStart<CustomAppStart>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<CustomAppStart>(provider.GetRequiredService<IMvxAppStart>());
        }

        [Fact]
        public void UseAppStart_CalledTwice_LastOneWins()
        {
            var builder = CreateTestBuilder();
            builder.UseAppStart<CustomAppStart>();
            builder.UseAppStart<AnotherCustomAppStart>();

            var provider = builder.Services.BuildServiceProvider();

            Assert.IsType<AnotherCustomAppStart>(provider.GetRequiredService<IMvxAppStart>());
        }

        [Fact]
        public void UseAppStart_NullFactory_Throws()
        {
            var builder = CreateTestBuilder();
            Assert.Throws<ArgumentNullException>(
                () => builder.UseAppStart<IMvxAppStart>(null!));
        }

        [Fact]
        public void UseAppStart_ReturnsBuilderForChaining()
        {
            var builder = CreateTestBuilder();
            var result = builder.UseAppStart<CustomAppStart>();
            Assert.Same(builder, result);
        }

        #endregion

        #region MvxHost subclassing

        [Fact]
        public void MvxHost_CanBeSubclassed_AndCreatedWithPublicConstructor()
        {
            var services = new ServiceCollection().BuildServiceProvider();
            var host = new CustomMvxHost(services);
            Assert.NotNull(host);
        }

        [Fact]
        public void CreateHost_Override_ReturnsCustomSubclass()
        {
            var builder = new CustomHostBuilder();
            var host = builder.Build();
            Assert.IsType<CustomMvxHost>(host);
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

        #endregion
    }
}
