using Example.Droid.Activities;

namespace Example.Droid.Fragments
{
    public abstract class BaseFragment : MvxFragment
    {
        private MvxActionBarDrawerToggle _drawerToggle;
        private Toolbar _toolbar;

        protected BaseFragment()
        {
            this.RetainInstance = true;
        }

        public MvxCachingFragmentCompatActivity ParentActivity => (MvxCachingFragmentCompatActivity) Activity;

        protected abstract int FragmentId { get; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(FragmentId, null);

            _toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (_toolbar != null)
            {
                ParentActivity.SetSupportActionBar(_toolbar);
                ParentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity, // host Activity
                    (ParentActivity as INavigationActivity).DrawerLayout, // DrawerLayout object
                    _toolbar, // nav drawer icon to replace 'Up' caret
                    Resource.String.drawer_open, // "open drawer" description
                    Resource.String.drawer_close // "close drawer" description
                );
                _drawerToggle.DrawerOpened +=
                    (object sender, ActionBarDrawerEventArgs e) => ((MainActivity) Activity).HideSoftKeyboard();
                (ParentActivity as INavigationActivity).DrawerLayout.AddDrawerListener(_drawerToggle);
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

    public abstract class BaseFragment<TViewModel> : BaseFragment where TViewModel : class, IMvxViewModel
    {
        public TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}