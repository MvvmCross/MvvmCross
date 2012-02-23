namespace Tutorial.UI.Droid.Controls.PullToRefresh
{
    public interface IOnPullingAction
    {
        void HandlePull(bool down, int height);
    }
}