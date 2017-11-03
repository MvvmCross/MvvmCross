using MvvmCross.Core.Views;

namespace MvvmCross.Uwp.Attributes
{
    public class MvxSplitViewPresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxSplitViewPresentationAttribute(SplitPanePosition position = SplitPanePosition.Content)
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
