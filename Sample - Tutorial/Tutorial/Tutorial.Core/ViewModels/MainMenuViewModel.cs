using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.ViewModels;


namespace Tutorial.Core.ViewModels
{
    public class MainMenuViewModel
        : MvxViewModel
    {
        public List<Type> Items { get; set; }

        public IMvxCommand ShowItemCommand
        {
            get
            {
                return new MvxRelayCommand<Type>((type) => this.RequestNavigate(type));
            }
        }

        public MainMenuViewModel()
        {
            Items = new List<Type>()
                        {
                            typeof(Lessons.SimpleTextPropertyViewModel),
                            typeof(Lessons.PullToRefreshViewModel),
                            typeof(Lessons.TipViewModel),
                            typeof(Lessons.CompositeViewModel),
                            typeof(Lessons.LocationViewModel)
                        };
        }
    }
}