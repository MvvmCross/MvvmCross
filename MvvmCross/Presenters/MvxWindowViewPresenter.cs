// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters;

/// <summary>
/// Routes presentation to an independent presenter for each window.
/// Register, unregister and present on the platform's UI thread.
/// </summary>
public abstract class MvxWindowViewPresenter : MvxViewPresenter
{
    private readonly Dictionary<string, WindowRegistration> _windows = new(StringComparer.Ordinal);
    private readonly ConditionalWeakTable<IMvxViewModel, WindowRegistration> _owners = new();
    private readonly HashSet<IMvxViewModel> _presenting = new(ReferenceEqualityComparer.Instance);

    // Clear Presenter on disconnect, including in registrations referenced by live ViewModels.
    private sealed class WindowRegistration(IMvxViewPresenter presenter)
    {
        public IMvxViewPresenter? Presenter { get; set; } = presenter;
    }

    /// <summary>Registers a presenter for a connected window. Window IDs must be unique.</summary>
    protected void RegisterPresenter(string windowId, IMvxViewPresenter presenter)
    {
        ArgumentException.ThrowIfNullOrEmpty(windowId);
        ArgumentNullException.ThrowIfNull(presenter);
        _windows.Add(windowId, new WindowRegistration(presenter));
    }

    /// <summary>
    /// Releases the presenter for a disconnected window. A subsequent connection may reuse
    /// the ID, but ViewModel instances from the old connection cannot be presented in the new window.
    /// </summary>
    public virtual bool UnregisterWindow(string windowId)
    {
        ArgumentNullException.ThrowIfNull(windowId);
        if (!_windows.Remove(windowId, out var registration))
            return false;

        registration.Presenter = null;
        return true;
    }

    /// <summary>Returns the presenter registered for a window, or null if disconnected.</summary>
    protected IMvxViewPresenter? GetPresenter(string windowId)
        => _windows.TryGetValue(windowId, out var registration) ? registration.Presenter : null;

    /// <summary>
    /// Finds the owner of a ViewModel instance. Override to support views created outside
    /// ViewModel-first navigation, such as application-specific state restoration.
    /// </summary>
    protected virtual IMvxViewPresenter? GetOwningPresenter(IMvxViewModel viewModel)
        => _owners.TryGetValue(viewModel, out var owner) ? owner.Presenter : null;

    /// <summary>
    /// Selects an explicitly named window, or the only connected window when no ID is supplied.
    /// Override to implement application-specific routing. Ambiguous requests return null.
    /// </summary>
    protected virtual IMvxViewPresenter? SelectPresenter(IDictionary<string, string>? presentationValues)
    {
        if (presentationValues?.TryGetValue(MvxWindowPresentationBundle.WindowIdKey, out var windowId) == true)
            return GetPresenter(windowId);

        return _windows.Count == 1 ? _windows.Values.First().Presenter : null;
    }

    [RequiresUnreferencedCode("Presentation uses view types and presentation attributes.")]
    public override async Task<bool> Show(MvxViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var presenter = SelectPresenter(request.PresentationValues);
        if (presenter == null)
            return false;

        var registration = _windows.Values.FirstOrDefault(w => ReferenceEquals(w.Presenter, presenter));
        if (registration == null)
            return false;

        var viewModel = (request as MvxViewModelInstanceRequest)?.ViewModelInstance;
        var addedOwner = false;
        if (viewModel != null)
        {
            if (_presenting.Contains(viewModel))
                return false;

            if (_owners.TryGetValue(viewModel, out var owner))
            {
                // A ViewModel instance belongs to one window connection, even after disconnect.
                if (!ReferenceEquals(owner, registration))
                    return false;
            }
            else
            {
                // Register before Show: view lifecycle callbacks can themselves navigate or close.
                _owners.Add(viewModel, registration);
                addedOwner = true;
            }
            _presenting.Add(viewModel);
        }

        var shown = false;
        try
        {
            shown = await presenter.Show(request).ConfigureAwait(true);
            return shown && registration.Presenter != null;
        }
        finally
        {
            if (viewModel != null)
            {
                _presenting.Remove(viewModel);
                if (!shown && addedOwner && registration.Presenter != null)
                    _owners.Remove(viewModel);
            }
        }
    }

    [RequiresUnreferencedCode("Presentation uses view types and presentation attributes.")]
    public override Task<bool> Close(IMvxViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        // Never try each presenter: a custom presenter may act on an unknown ViewModel.
        var presenter = GetOwningPresenter(viewModel);
        return presenter != null
            ? presenter.Close(viewModel)
            : Task.FromResult(false);
    }

    [RequiresUnreferencedCode("Presentation uses view types and presentation attributes.")]
    public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
    {
        ArgumentNullException.ThrowIfNull(hint);
        if (await HandlePresentationChange(hint).ConfigureAwait(true))
            return true;

        var presenter = hint is MvxClosePresentationHint close
            ? GetOwningPresenter(close.ViewModelToClose)
            : SelectPresenter(hint.Body?.Data);
        return presenter != null && await presenter.ChangePresentation(hint).ConfigureAwait(true);
    }
}
