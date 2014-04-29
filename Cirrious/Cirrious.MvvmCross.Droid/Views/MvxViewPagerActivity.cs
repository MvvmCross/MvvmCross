using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentPagerAdapter = Android.Support.V4.App.FragmentPagerAdapter;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using Android.Support.V4.View;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxViewPagerActivity : MvxFragmentActivity
    {    
        List<MvxRootFragment> _rootFragments;
        public List<MvxRootFragment> RootFragments
        {
            get
            {
                if (_rootFragments == null)
                {
                    _rootFragments = new List<MvxRootFragment>();
                }
                return _rootFragments;
            }
            set { _rootFragments = value; }
        }

        MvxViewPagerAdapter _adapter;
        public MvxViewPagerAdapter Adapter
        {
            get { return _adapter; }
            set { _adapter = value; }
        }

        ViewPager _viewPager;
        public ViewPager ViewPager
        {
            get { return _viewPager; }
            set { _viewPager = value; }
        }

        int _incrementalId = 26679098;
        public int IncrementalId
        {
            get { return _incrementalId++; }
            set { _incrementalId = value; }
        }

        public MvxViewPagerActivity() : base()
        {
            Adapter = new MvxViewPagerAdapter(SupportFragmentManager, this);
        }

        public void AddPage(MvxFragment f, int iconId, string title, int containerId)
        {
            MvxRootFragment rf = new MvxRootFragment(this);
            rf.IconId = iconId;
            rf.Title = title;
            rf.ContainerId = containerId;
            rf.Stack.Push(f);

            RootFragments.Add(rf);
        }

        public MvxRootFragment GetCurrentRootFragment()
        {
            if (_rootFragments.Count == 0)
                throw new Exception("No child root fragments found");
            if (_viewPager == null)
                throw new Exception("No view pager found");
            return _rootFragments[ViewPager.CurrentItem];
        }
    }

    public class MvxRootFragment : Fragment
    {
        MvxViewPagerActivity _context;

        LinearLayout _layout;

        string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        int _iconId;
        public int IconId
        {
            get { return _iconId; }
            set { _iconId = value; }
        }

        int _containerId;
        public int ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        Stack<MvxFragment> _stack;
        public Stack<MvxFragment> Stack
        {
            get
            {
                if (_stack == null)
                {
                    _stack = new Stack<MvxFragment>();
                }
                return _stack;
            }
            set { _stack = value; }
        }

        public MvxRootFragment(MvxViewPagerActivity context)
        {
            _context = context;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            /*if (_layout == null)
            {
                _layout = new LinearLayout(_context);
                _layout.Id = _context.IncrementalId;
                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent);
                _layout.LayoutParameters = lp;
                _layout.SetBackgroundColor(Android.Graphics.Color.Orange);
            }*/
            _layout = (LinearLayout)inflater.Inflate(ContainerId, container, false);

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Fragment firstChildFragment = Stack.Peek();
            ft.Replace(_layout.Id, firstChildFragment);
            ft.Commit();

            return _layout;
        }

        public int GetContainerId()
        {
            return _layout.Id;
        }
    }

    public class MvxViewPagerAdapter : FragmentPagerAdapter
    {
        protected MvxViewPagerActivity _activity;

        public MvxViewPagerAdapter (FragmentManager fm, MvxViewPagerActivity activity) : base(fm)
        {
            _activity = activity;
        }

        public override Fragment GetItem (int position)
        {
            if (_activity.RootFragments.Count == 0)
            {
                throw new Exception("There is no Fragments to show on the ViewPager");
            }
            return _activity.RootFragments[position];
        }

        public virtual string GetTitle(int position)
        {
            return _activity.RootFragments[position].Title;
        }

        public virtual int GetIconId(int position)
        {
            return _activity.RootFragments[position].IconId;
        }

        public override int Count {
            get {
                return _activity.RootFragments.Count;
            }
        }

        /*public override int GetItemPosition(Java.Lang.Object @object)
        {
            return PositionNone;
        }*/
    }
}

