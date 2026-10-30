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

namespace MvvmCross.UnitTest.Presenters;

[RequiresUnreferencedCode("Tests exercise presentation with statically referenced ViewModels and substitute presenters.")]
[NotInParallel]
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

    [Test]
    public async Task UnqualifiedNavigationRequiresExactlyOneWindow()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        await Assert.That(await router.Show(Request())).IsFalse();
        router.Register("first", first);
        await Assert.That(await router.Show(Request())).IsTrue();
        router.Register("second", CreatePresenter());
        await Assert.That(await router.Show(Request())).IsFalse();
        await first.Received(1).Show(Arg.Any<MvxViewModelRequest>());
    }

    [Test]
    public async Task ExplicitWindowReceivesOriginalRequestAndOtherPresentationValues()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var second = CreatePresenter();
        router.Register("first", first);
        router.Register("second", second);
        var request = Request("second");
        request.PresentationValues!["OtherHint"] = "value";

        await Assert.That(await router.Show(request)).IsTrue();
        await second.Received(1).Show(request);
        await first.DidNotReceive().Show(Arg.Any<MvxViewModelRequest>());
        await Assert.That(request.PresentationValues["OtherHint"]).IsEqualTo("value");
    }

    [Test]
    [Arguments("missing")]
    [Arguments("")]
    [Arguments("FIRST")]
    public async Task InvalidExplicitWindowNeverFallsBack(string id)
    {
        var router = new Presenter();
        var first = CreatePresenter();
        router.Register("first", first);
        var request = Request();
        request.PresentationValues = new Dictionary<string, string> { [MvxWindowPresentationBundle.WindowIdKey] = id };
        await Assert.That(await router.Show(request)).IsFalse();
        await first.DidNotReceive().Show(Arg.Any<MvxViewModelRequest>());
    }

    [Test]
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

        await Assert.That(await router.ChangePresentation(new MvxClosePresentationHint(firstModel,
            new MvxWindowPresentationBundle("second")))).IsTrue();
        await Assert.That(await router.Close(secondModel)).IsTrue();
        await Assert.That(await router.Close(new ViewModel())).IsFalse();
        await first.Received(1).Close(firstModel);
        await second.Received(1).Close(secondModel);
        await second.DidNotReceive().Close(firstModel);
    }

    [Test]
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
        await Assert.That(await router.Show(Request("second"))).IsTrue();
        pending.SetResult(true);
        await Assert.That(await firstTask).IsTrue();
        await Assert.That(await router.Close(request.ViewModelInstance!)).IsTrue();
        await first.Received(1).Close(request.ViewModelInstance!);
        await second.DidNotReceive().Close(Arg.Any<IMvxViewModel>());
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
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

        await Assert.That(await showing).IsEqualTo(succeeds);
        await Assert.That(overlap).IsFalse();
        await first.Received(1).Show(Arg.Any<MvxViewModelRequest>());
        await Assert.That(await router.Close(model)).IsEqualTo(succeeds);
        await Assert.That(await router.Show(Request("second", model))).IsEqualTo(!succeeds);
        if (succeeds)
            await Assert.That(await router.Show(Request("first", model))).IsTrue();
    }

    [Test]
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
        await Assert.That(overlap).IsFalse();
        await Assert.That(await router.Close(model)).IsFalse();
        await Assert.That(await router.Show(Request("second", model))).IsTrue();
    }

    [Test]
    [Arguments(true, false)]
    [Arguments(false, false)]
    [Arguments(false, true)]
    public async Task DisconnectDuringPresentationDoesNotResurrectOwnership(bool succeeds, bool throws)
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var pending = new TaskCompletionSource<bool>();
        first.Show(Arg.Any<MvxViewModelRequest>()).Returns(pending.Task);
        router.Register("scene", first);
        var request = Request("scene");
        var showing = router.Show(request);
        await Assert.That(router.UnregisterWindow("scene")).IsTrue();
        await Assert.That(router.UnregisterWindow("scene")).IsFalse();
        var replacement = CreatePresenter();
        router.Register("scene", replacement);
        if (throws)
            pending.SetException(new InvalidOperationException());
        else
            pending.SetResult(succeeds);

        if (throws)
            await Assert.ThrowsAsync<InvalidOperationException>(() => showing);
        else
            await Assert.That(await showing).IsFalse();
        await Assert.That(await router.Close(request.ViewModelInstance!)).IsFalse();
        await Assert.That(await router.Show(request)).IsFalse();
        await Assert.That(await router.Show(Request("scene"))).IsTrue();
        await replacement.DidNotReceive().Close(Arg.Any<IMvxViewModel>());
    }

    [Test]
    public async Task SameInstanceCannotBePresentedInTwoWindows()
    {
        var router = new Presenter();
        router.Register("first", CreatePresenter());
        var second = CreatePresenter();
        router.Register("second", second);
        var model = new ViewModel();
        await Assert.That(await router.Show(Request("first", model))).IsTrue();
        await Assert.That(await router.Show(Request("second", model))).IsFalse();
        await second.DidNotReceive().Show(Arg.Any<MvxViewModelRequest>());
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
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
            await Assert.That(await router.Show(Request("first", model))).IsFalse();

        await Assert.That(await router.Close(model)).IsFalse();
        await Assert.That(await router.Show(Request("second", model))).IsTrue();
    }

    [Test]
    public async Task OwnershipIsAvailableInsideViewLifecycleCallbacks()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        router.Register("first", first);
        var request = Request("first");
        first.Show(request).Returns(_ => router.Close(request.ViewModelInstance!));
        await Assert.That(await router.Show(request)).IsTrue();
        await first.Received(1).Close(request.ViewModelInstance!);
    }

    [Test]
    public async Task HintsRequireWindowContextAndCustomHandlersStillWork()
    {
        var router = new Presenter();
        var first = CreatePresenter();
        var second = CreatePresenter();
        router.Register("first", first);
        router.Register("second", second);
        await Assert.That(await router.ChangePresentation(new Hint())).IsFalse();
        var hint = new Hint(new MvxWindowPresentationBundle("second"));
        await Assert.That(await router.ChangePresentation(hint)).IsTrue();
        await second.Received(1).ChangePresentation(hint);
        await first.DidNotReceive().ChangePresentation(Arg.Any<MvxPresentationHint>());
        router.AddPresentationHintHandler<Hint>(_ => Task.FromResult(true));
        await Assert.That(await router.ChangePresentation(new Hint())).IsTrue();
    }

    [Test]
    public void DuplicateWindowCannotReplaceExistingPresenter()
    {
        var router = new Presenter();
        router.Register("scene", CreatePresenter());
        Assert.Throws<ArgumentException>(() => router.Register("scene", CreatePresenter()));
    }

    [Test]
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
        await Assert.That(await router.ChangePresentation(hint)).IsFalse();
        await first.Received(1).ChangePresentation(hint);
    }

    [Test]
    [SuppressMessage("Major Code Smell", "S1215", Justification = "Verifies that disconnected windows are collectable while their ViewModels remain alive.")]
    public async Task DisconnectReleasesPresenterWhileItsViewModelIsStillAlive()
    {
        var router = new Presenter();
        var model = new ViewModel();
        var reference = await RegisterAndDisconnect(router, model);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        await Assert.That(reference.IsAlive).IsFalse();
        GC.KeepAlive(router);
        GC.KeepAlive(model);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> RegisterAndDisconnect(Presenter router, IMvxViewModel model)
    {
        var window = new StatelessPresenter();
        var reference = new WeakReference(window);
        router.Register("scene", window);
        await Assert.That(await router.Show(Request("scene", model))).IsTrue();
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
