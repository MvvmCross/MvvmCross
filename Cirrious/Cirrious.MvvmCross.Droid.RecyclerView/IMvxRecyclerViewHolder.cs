using Cirrious.MvvmCross.Binding.BindingContext;

namespace Cirrious.MvvmCross.Droid.RecyclerView
{
    public interface IMvxRecyclerViewHolder : IMvxBindingContextOwner
    {
        object DataContext { get; set; }

        void OnAttachedToWindow();
        void OnDetachedFromWindow();
    }
}