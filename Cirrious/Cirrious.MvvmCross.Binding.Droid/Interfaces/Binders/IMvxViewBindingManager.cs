using System;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders
{
    public interface IMvxViewBindingManager
    {
        /// <summary>
        /// Binds the view and it's children to a data source. You must always <see cref="M:UnbindView"/> the view as
        /// soon as you are ready to discard it.
        /// 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Data source can be specified in two ways, either specifying it using the <paramref name="dataSource"/>
        /// parameter or having the data source attached to the view as a tag. When you specify it using the 
        /// <paramref name="dataSource"/> parameter it also attaches it to the tag.
        /// </para>
        /// 
        /// <para>
        /// The data source is used for the item it is specified for and it's decendants. The child items can, however,
        /// override the data source and have it's decendants bind to a different data source.
        /// </para>
        /// 
        /// <para>
        /// If there is no data source attached to the view but it has binding instructions, a warning will be logged
        /// and the bindings ignored.
        /// </para>
        /// 
        /// <para>
        /// Usually you call it manually only when adding views to the view hierarchy after the data source has been
        /// set already. If the view isn't in the view hierarchy then the view will be unbound automatically on next
        /// invocation of <see cref="M:RebindViews"/>.
        /// </para>
        /// </remarks>
        /// <param name="view">View to bind.</param>
        /// <param name="dataSource">
        /// The data source to bind to the view, null to use the one attached to the view already.
        /// </param>
        void BindView (View view, object dataSource = null);

        /// <summary>
        /// Removes the bindings for the view and it's decendants.
        /// </summary>
        /// 
        /// <remarks>
        /// If the view is still present in the <see cref="M:RootView"/> hieararchy it will be automatically
        /// rebound on the next invocation of <see cref="M:RebindViews"/>. And will be removed correctly when
        /// the <see cref="M:RootView"/> is disposed of.
        /// </remarks>
        /// <param name="view">View to unbind.</param>
        void UnbindView (View view);

        /// <summary>
        /// Traverses the <see cref="M:RootView"/> view hierarchy and makes sure that the bindings are up to date.
        /// </summary>
        /// <remarks>
        /// This method should not be called very often as traversing the view hieararchy is fairly expensive operation.
        /// </remarks>
        void RebindViews ();

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
        /// Unbinds all active bindings. This should be called when the resources used by bindings need to be released.
        /// </summary>
        void UnbindAll ();
    }
}