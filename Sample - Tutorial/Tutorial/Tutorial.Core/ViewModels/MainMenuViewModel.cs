using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;


namespace Tutorial.Core.ViewModels
{
    public class MainMenuViewModel
        : MvxViewModel
    {
        public List<Type> Items { get; set; }

        public ICommand ShowItemCommand
        {
            get
            {
                return new MvxCommand<Type>((type) => DoShowItem(type));
            }
        }

        public void DoShowItem(Type itemType)
        {
            this.ShowViewModel(itemType);
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