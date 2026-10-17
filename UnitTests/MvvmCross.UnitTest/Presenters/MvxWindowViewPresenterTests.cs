// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using NSubstitute;
using NSubstitute.Extensions;
using Xunit;

namespace MvvmCross.UnitTest.Presenters;

[RequiresUnreferencedCode("Tests exercise presentation with statically referenced ViewModels and substitute presenters.")]
[Collection("Window presenter lifetime")]
public class MvxWindowViewPresenterTests
{
    private sealed class Presenter : MvxWindowViewPresenter
    {
        public void Register(string id, IMvxViewPresenter presenter) => RegisterPresenter(id, presenter);
    }

    private sealed class ViewModel : MvxViewModel;
    private sealed class Hint(MvxBundle? bundle = null) : MvxPresentationHint(bundle);

    private static IMvxViewPresenter CreatePresenter()
    {
        var presenter = Substitute.For<IMvxViewPresenter>();
        presenter.Show(Arg.Any<MvxViewModelRequest>()).Returns(true);
        presenter.Close(Arg.Any<IMvxViewModel>()).Returns(true);
        presenter.ChangePresentation(Arg.Any<MvxPresentationHint>()).Returns(true);
        presenter.ChangePresentation(Arg.Any<MvxClosePresentationHint>())
            .Returns(call => presenter.Close(call.Arg<MvxClosePresentationHint>().ViewModelToClose));
        return presenter;
    }

    private static MvxViewModelInstanceRequest Request(string? windowId = null, IMvxViewModel? viewModel = null)
        => new(viewModel ?? new ViewModel())
        {
            PresentationValues = windowId == null ? null : new MvxWindowPresentationBundle(windowId).Data
        };

    [Fact]
    public async Task UnqualifiedNavigationRequiresExactlyOneWindow()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        Assert.False(await router.Show(Request()));
        router.Register("first", first);
        Assert.True(await router.Show(Request()));
        router.Register("second", CreatePresenter());
        Assert.False(await router.Show(Request()));
        await first.Received(1).Show(Arg.Any<MvxViewModelRequest>());
    }

    [Fact]
    public async Task ExplicitWindowReceivesOriginalRequestAndOtherPresentationValues()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var second = CreatePresenter();
        router.Register("first", first);
        router.Register("second", second);
        var request = Request("second");
        request.PresentationValues!["OtherHint"] = "value";

        Assert.True(await router.Show(request));
        await second.Received(1).Show(request);
        await first.DidNotReceive().Show(Arg.Any<MvxViewModelRequest>());
        Assert.Equal("value", request.PresentationValues["OtherHint"]);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("")]
    [InlineData("FIRST")]
    public async Task InvalidExplicitWindowNeverFallsBack(string id)
    {
        var router = new Presenter();
        var first = CreatePresenter();
        router.Register("first", first);
        var request = Request();
        request.PresentationValues = new Dictionary<string, string> { [MvxWindowPresentationBundle.WindowIdKey] = id };
        Assert.False(await router.Show(request));
        await first.DidNotReceive().Show(Arg.Any<MvxViewModelRequest>());
    }

    [Fact]
    public async Task CloseUsesInstanceOwnershipEvenWhenHintNamesAnotherWindow()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var second = CreatePresenter();
        router.Register("first", first);
        router.Register("second", second);
        var firstModel = new ViewModel();
        var secondModel = new ViewModel();
        await router.Show(Request("first", firstModel));
        await router.Show(Request("second", secondModel));

        Assert.True(await router.ChangePresentation(new MvxClosePresentationHint(firstModel,
            new MvxWindowPresentationBundle("second"))));
        Assert.True(await router.Close(secondModel));
        Assert.False(await router.Close(new ViewModel()));
        await first.Received(1).Close(firstModel);
        await second.Received(1).Close(secondModel);
        await second.DidNotReceive().Close(firstModel);
    }

    [Fact]
    public async Task PendingPresentationKeepsItsWindowWhenAnotherWindowNavigates()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var second = CreatePresenter();
        var pending = new TaskCompletionSource<bool>();
        first.Show(Arg.Any<MvxViewModelRequest>()).Returns(pending.Task);
        router.Register("first", first);
        router.Register("second", second);
        var request = Request("first");
        var firstTask = router.Show(request);
        Assert.True(await router.Show(Request("second")));
        pending.SetResult(true);
        Assert.True(await firstTask);
        Assert.True(await router.Close(request.ViewModelInstance!));
        await first.Received(1).Close(request.ViewModelInstance!);
        await second.DidNotReceive().Close(Arg.Any<IMvxViewModel>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SameInstanceCannotOverlapPresentations(bool succeeds)
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var pending = new TaskCompletionSource<bool>();
        first.Show(Arg.Any<MvxViewModelRequest>()).Returns(pending.Task, Task.FromResult(true));
        router.Register("first", first);
        router.Register("second", CreatePresenter());
        var model = new ViewModel();
        var showing = router.Show(Request("first", model));
        var overlap = await router.Show(Request("first", model));
        pending.SetResult(succeeds);

        Assert.Equal(succeeds, await showing);
        Assert.False(overlap);
        await first.Received(1).Show(Arg.Any<MvxViewModelRequest>());
        Assert.Equal(succeeds, await router.Close(model));
        Assert.Equal(!succeeds, await router.Show(Request("second", model)));
        if (succeeds)
            Assert.True(await router.Show(Request("first", model)));
    }

    [Fact]
    public async Task ThrowingPendingPresentationAllowsRetry()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var pending = new TaskCompletionSource<bool>();
        first.Show(Arg.Any<MvxViewModelRequest>()).Returns(pending.Task, Task.FromResult(true));
        router.Register("first", first);
        router.Register("second", CreatePresenter());
        var model = new ViewModel();
        var showing = router.Show(Request("first", model));
        var overlap = await router.Show(Request("first", model));
        pending.SetException(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(() => showing);
        Assert.False(overlap);
        Assert.False(await router.Close(model));
        Assert.True(await router.Show(Request("second", model)));
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(false, true)]
    public async Task DisconnectDuringPresentationDoesNotResurrectOwnership(bool succeeds, bool throws)
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var pending = new TaskCompletionSource<bool>();
        first.Show(Arg.Any<MvxViewModelRequest>()).Returns(pending.Task);
        router.Register("scene", first);
        var request = Request("scene");
        var showing = router.Show(request);
        Assert.True(router.UnregisterWindow("scene"));
        Assert.False(router.UnregisterWindow("scene"));
        var replacement = CreatePresenter();
        router.Register("scene", replacement);
        if (throws)
            pending.SetException(new InvalidOperationException());
        else
            pending.SetResult(succeeds);

        if (throws)
            await Assert.ThrowsAsync<InvalidOperationException>(() => showing);
        else
            Assert.False(await showing);
        Assert.False(await router.Close(request.ViewModelInstance!));
        Assert.False(await router.Show(request));
        Assert.True(await router.Show(Request("scene")));
        await replacement.DidNotReceive().Close(Arg.Any<IMvxViewModel>());
    }

    [Fact]
    public async Task SameInstanceCannotBePresentedInTwoWindows()
    {
        var router = new Presenter();
        router.Register("first", CreatePresenter());
        var second = CreatePresenter();
        router.Register("second", second);
        var model = new ViewModel();
        Assert.True(await router.Show(Request("first", model)));
        Assert.False(await router.Show(Request("second", model)));
        await second.DidNotReceive().Show(Arg.Any<MvxViewModelRequest>());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task FailedPresentationDoesNotClaimViewModel(bool throws)
    {
        var router = new Presenter();
        var first = CreatePresenter();
        first.Show(Arg.Any<MvxViewModelRequest>()).Returns(throws
            ? Task.FromException<bool>(new InvalidOperationException())
            : Task.FromResult(false));
        router.Register("first", first);
        router.Register("second", CreatePresenter());
        var model = new ViewModel();

        if (throws)
            await Assert.ThrowsAsync<InvalidOperationException>(() => router.Show(Request("first", model)));
        else
            Assert.False(await router.Show(Request("first", model)));

        Assert.False(await router.Close(model));
        Assert.True(await router.Show(Request("second", model)));
    }

    [Fact]
    public async Task OwnershipIsAvailableInsideViewLifecycleCallbacks()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        router.Register("first", first);
        var request = Request("first");
        first.Show(request).Returns(_ => router.Close(request.ViewModelInstance!));
        Assert.True(await router.Show(request));
        await first.Received(1).Close(request.ViewModelInstance!);
    }

    [Fact]
    public async Task HintsRequireWindowContextAndCustomHandlersStillWork()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var second = CreatePresenter();
        router.Register("first", first);
        router.Register("second", second);
        Assert.False(await router.ChangePresentation(new Hint()));
        var hint = new Hint(new MvxWindowPresentationBundle("second"));
        Assert.True(await router.ChangePresentation(hint));
        await second.Received(1).ChangePresentation(hint);
        await first.DidNotReceive().ChangePresentation(Arg.Any<MvxPresentationHint>());
        router.AddPresentationHintHandler<Hint>(_ => Task.FromResult(true));
        Assert.True(await router.ChangePresentation(new Hint()));
    }

    [Fact]
    public void DuplicateWindowCannotReplaceExistingPresenter()
    {
        var router = new Presenter();
        router.Register("scene", CreatePresenter());
        Assert.Throws<ArgumentException>(() => router.Register("scene", CreatePresenter()));
    }

    [Fact]
    public async Task CloseHintPreservesTheOwningPresentersCustomHandler()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        router.Register("first", first);
        router.Register("second", CreatePresenter());
        var model = new ViewModel();
        await router.Show(Request("first", model));
        var hint = new MvxClosePresentationHint(model);
        first.Configure().ChangePresentation(hint).Returns(false);
        Assert.False(await router.ChangePresentation(hint));
        await first.Received(1).ChangePresentation(hint);
    }

    [Fact]
    [SuppressMessage("Major Code Smell", "S1215", Justification = "Verifies that disconnected windows are collectable while their ViewModels remain alive.")]
    public void DisconnectReleasesPresenterWhileItsViewModelIsStillAlive()
    {
        var router = new Presenter();
        var model = new ViewModel();
        var reference = RegisterAndDisconnect(router, model);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(reference.IsAlive);
        GC.KeepAlive(router);
        GC.KeepAlive(model);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference RegisterAndDisconnect(Presenter router, IMvxViewModel model)
    {
        var window = new StatelessPresenter();
        var reference = new WeakReference(window);
        router.Register("scene", window);
        Assert.True(router.Show(Request("scene", model)).GetAwaiter().GetResult());
        router.UnregisterWindow("scene");
        return reference;
    }

    private sealed class StatelessPresenter : MvxViewPresenter
    {
        [RequiresUnreferencedCode("Implements the presentation contract for testing.")]
        public override Task<bool> Show(MvxViewModelRequest request) => Task.FromResult(true);
        [RequiresUnreferencedCode("Implements the presentation contract for testing.")]
        public override Task<bool> Close(IMvxViewModel viewModel) => Task.FromResult(true);
        [RequiresUnreferencedCode("Implements the presentation contract for testing.")]
        public override Task<bool> ChangePresentation(MvxPresentationHint hint) => Task.FromResult(true);
    }
}

// Forced collection in the lifetime test must not run alongside other tests using weak references.
[CollectionDefinition("Window presenter lifetime", DisableParallelization = true)]
public class WindowPresenterLifetimeCollection;
