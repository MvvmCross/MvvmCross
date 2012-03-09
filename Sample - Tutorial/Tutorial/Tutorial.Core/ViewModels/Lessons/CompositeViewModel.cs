using System;
using Cirrious.MvvmCross.ViewModels;

namespace Tutorial.Core.ViewModels.Lessons
{
    public class CompositeViewModel
        : MvxViewModel
    {
        public TipViewModel Tip { get; private set; }
        public PullToRefreshViewModel Pull { get; private set; }
        public SimpleTextPropertyViewModel Text { get; private set; }

        public CompositeViewModel()
        {
            Pull = new PullToRefreshViewModel();
            Tip = new TipViewModel();
            Text = new SimpleTextPropertyViewModel();
        }
    }
}