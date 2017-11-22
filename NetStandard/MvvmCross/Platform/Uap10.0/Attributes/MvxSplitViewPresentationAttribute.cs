using MvvmCross.Core.Views;

namespace MvvmCross.Uwp.Attributes
{
    public class MvxSplitViewPresentationAttribute : MvxBasePresentationAttribute
    {

        public MvxSplitViewPresentationAttribute() : this(SplitPanePosition.Content)
        {
            
        }

        public MvxSplitViewPresentationAttribute(SplitPanePosition position)
        {
            Position = position;
        }

        public SplitPanePosition Position { get; set; }
    }

    public enum SplitPanePosition
    {
        Pane,
        Content
    }

}
