using System;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Binders;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders
{
    public interface IMvxViewBindingManager
    {
        /// <summary>
        /// Gets the root view for the binding manager.
        /// 
        /// <para>
        /// Root view is the top most element in the view hierarchy. It is usually the one which gets set by
        /// <c>activity.SetContentView(...)</c>.
        /// </para>
        /// </summary>
        /// <example>
        /// <para>
        /// To rebind everything in the view hieararchy, we call the <see cref="M:BindView"/> method.
        /// </para>
        /// 
        /// <code>
        /// public override void OnStart() {
        ///     base.OnStart();
        ///     BindingManager.BindView(BindingManager.RootView, DefaultBindingSource);
        /// }
        /// </code>
        /// </example>
        /// <value>The root view.</value>
        View RootView { get; }

        /// <summary>
        /// Binds the view and it's children to a data source. You must always <see cref="M:UnbindView"/> the view
        /// as soon as you are ready to discard it.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// Most of the time you should not specify the <paramref name="dataSource"/> as it will mark the inherited
        /// data source to be overridden for this view and its decendants. To avoid headaches you should only
        /// override the data source when you want to use a different data source.
        /// </para>
        /// 
        /// <para>
        /// When the <paramref name="dataSource"/> is null, the data binding source will be determined from the
        /// <see cref="MvxViewBindingTag"/> if it has been set there or by walking the chain of parents for this view.
        /// </para>
        /// 
        /// <para>
        /// The data source is used for the view it is specified for and it's decendants. The child items can,
        /// however, override the data source and have it's decendants bind to that instead. This can
        /// be done by calling the <see cref="M:BindView"/> method with the specific data source.
        /// Alternatively you can do that by setting the new data source in the <see cref="MvxViewBindingTag"/> and
        /// then later calling the <see cref="M:BindView"/> method to update the bindings.
        /// </para>
        /// 
        /// <para>
        /// If no data source is found and the view has binding instructions, a warning will be logged
        /// and the bindings ignored.
        /// </para>
        /// 
        /// <para>
        /// When invoking this method with <see cref="P:RootView"/> we also clean up bound views which are not
        /// found in the view hierarchy. You should not count on this behaviour and still use
        /// <see cref="M:UnbindView"/> to unbind everything that you are destroying/throwing away.
        /// </para>
        /// 
        /// <para>
        /// Usually you only call it yourself when manipulating bound views after the data source has been set.
        /// If the view isn't in the view hierarchy then the view will be unbound automatically on next
        /// invocation of <see cref="M:BindView"/> on <see cref="P:RootView"/>.
        /// </para>
        /// </remarks>
        /// 
        /// <example>
        /// <para>
        /// To replace a data bound view from the view hieararchy:
        /// </para>
        /// 
        /// <code>
        /// Activity activity = ...;
        /// IMvxViewBindingManager bindings = ...;
        /// 
        /// var layout = activity.FindViewById<LinearLayout>(Resource.Id.LinearLayout);
        /// var view = layout.FindViewById<View>(Resrouce.Id.TestView);
        /// 
        /// // Clean up everything:
        /// layout.RemoveView(view);
        /// bindings.UnbindView(view);
        /// 
        /// // Create new layout and activate bindings:
        /// view = activity.LayoutInflater.Inflate(Resource.Layout.TestLayout, layout);
        /// bindings.BindView(view);
        /// </code>
        /// </example>
        /// 
        /// <param name="view">View to bind.</param>
        /// <param name="dataSource">
        /// The data source to bind to the view, null to use the one attached to the view already.
        /// </param>
        /// 
        /// <seealso cref="T:MvxViewBindingTag"/>
        void BindView (View view, object dataSource = null);

        /// <summary>
        /// Removes the bindings for the view and it's decendants.
        /// </summary>
        /// 
        /// <remarks>
        /// If the view is still present in the <see cref="P:RootView"/> view hieararchy it will be
        /// automatically rebound next time <see cref="M:BindView"/> is called for one of its parents.
        /// It will be removed correctly when the parent view is disposed of correctly.
        /// </remarks>
        /// <param name="view">View to unbind.</param>
        void UnbindView (View view);

        /// <summary>
        /// Registers a custom binding which should be disposed of automatically when the <see cref="M:UnbindAll"/>
        /// method is called.
        /// </summary>
        /// <param name="binding">Binding to register.</param>
        void AddBinding (IMvxBinding binding);

        /// <summary>
        /// Removes the registration of a custom binding. After this you must manually dispose of the binding when
        /// done using it.
        /// </summary>
        /// <param name="binding">Binding to unregister.</param>
        void RemoveBinding (IMvxBinding binding);

        /// <summary>
        /// Unbinds all active bindings (both view and registered).
        /// This should be called when the resources used by bindings need to be released.
        /// </summary>
        void UnbindAll ();
    }
}