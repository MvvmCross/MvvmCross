using Example.Droid.Activities;

namespace Example.Droid.Fragments
{
    public abstract class BaseStateFragment : MvxFragment
    {
        private MvxActionBarDrawerToggle _drawerToggle;
        private Toolbar _toolbar;

        protected abstract int FragmentId { get; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(FragmentId, null);

            _toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (_toolbar != null)
            {
                ((MainActivity) Activity).SetSupportActionBar(_toolbar);
                ((MainActivity) Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity, // host Activity
                    ((MainActivity) Activity).DrawerLayout, // DrawerLayout object
                    _toolbar, // nav drawer icon to replace 'Up' caret
                    Resource.String.drawer_open, // "open drawer" description
                    Resource.String.drawer_close // "close drawer" description
                );

                ((MainActivity) Activity).DrawerLayout.AddDrawerListener(_drawerToggle);
            }

            return view;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (_toolbar != null)
                _drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (_toolbar != null)
                _drawerToggle.SyncState();
        }
    }

    public abstract class BaseStateFragment<TViewModel> : BaseStateFragment where TViewModel : class, IMvxViewModel
    {
        public TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}