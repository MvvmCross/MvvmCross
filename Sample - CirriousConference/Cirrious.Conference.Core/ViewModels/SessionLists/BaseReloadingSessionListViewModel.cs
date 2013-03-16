namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public abstract class BaseReloadingSessionListViewModel<T>
        : BaseSessionListViewModel<T>
    {
        public override void Start()
        {
            LoadSessions();
            base.Start();
        }

        protected override void RepositoryOnLoadingChanged()
        {
            LoadSessions();
            base.RepositoryOnLoadingChanged();
        }

        protected abstract void LoadSessions();
    }
}