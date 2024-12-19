// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Runtime;
using Android.Views;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.DroidX.RecyclerView;

[Register("mvvmcross.droidx.recyclerview.MvxRecyclerViewHolder")]
public class MvxRecyclerViewHolder
    : AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder, IMvxRecyclerViewHolder
{
    public event EventHandler<EventArgs>? Click;
    public event EventHandler<EventArgs>? LongClick;

    private IMvxBindingContext? _bindingContext;
    private object? _cachedDataContext;
    private IDisposable? _itemViewClickSubscription, _itemViewLongClickSubscription;

    public IMvxBindingContext? BindingContext
    {
        get => _bindingContext;
        set => throw new NotImplementedException("BindingContext is readonly in the list item");
    }

    public int Id { get; set; }

    public object? DataContext
    {
        get => _bindingContext?.DataContext;
        set
        {
            if (_bindingContext != null)
                _bindingContext.DataContext = value;

            // This is just a precaution.  If we've set the DataContext to something
            // then we don't need to have the old one still cached.
            _cachedDataContext = null;
        }
    }

    public MvxRecyclerViewHolder(View itemView, IMvxAndroidBindingContext context)
        : base(itemView)
    {
        _bindingContext = context;
    }

    [Preserve(Conditional = true)]
    protected MvxRecyclerViewHolder(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    public virtual void OnAttachedToWindow()
    {
        if (_cachedDataContext != null && DataContext == null && _bindingContext != null)
            _bindingContext.DataContext = _cachedDataContext;

        _cachedDataContext = null;

        _itemViewClickSubscription ??= ItemView.WeakSubscribe(nameof(View.Click), OnItemViewClick);
        _itemViewLongClickSubscription ??=
            ItemView.WeakSubscribe<View, View.LongClickEventArgs>(nameof(View.LongClick), OnItemViewLongClick);
    }

    public virtual void OnDetachedFromWindow()
    {
        _itemViewClickSubscription?.Dispose();
        _itemViewClickSubscription = null;
        _itemViewLongClickSubscription?.Dispose();
        _itemViewLongClickSubscription = null;

        _cachedDataContext = DataContext;
        if (_bindingContext != null)
            _bindingContext.DataContext = null;
    }

    protected virtual void OnItemViewClick(object? sender, EventArgs e)
    {
        Click?.Invoke(this, e);
    }

    protected virtual void OnItemViewLongClick(object? sender, EventArgs e)
    {
        LongClick?.Invoke(this, e);
    }

    public virtual void OnViewRecycled()
    {
        _cachedDataContext = null;
        if (_bindingContext != null)
            _bindingContext.DataContext = null;
    }

    protected override void Dispose(bool disposing)
    {
        _itemViewClickSubscription?.Dispose();
        _itemViewClickSubscription = null;
        _itemViewLongClickSubscription?.Dispose();
        _itemViewLongClickSubscription = null;

        _cachedDataContext = null;

        if (_bindingContext != null)
        {
            _bindingContext.DataContext = null;
            _bindingContext.ClearAllBindings();
            _bindingContext.DisposeIfDisposable();
            _bindingContext = null;
        }

        base.Dispose(disposing);
    }
}
