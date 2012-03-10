using System.Collections.Generic;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers.ViewModels
{
    public class CategoryDataViewModel
        : BaseViewModel
    {
        public string ListName { get; set; }
        public string DisplayName { get; set; }

        public string ListNameEncoded
        {
            get { return ListName.ToLower().Replace(' ', '-'); }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public IMvxCommand ShowCategoryCommand
        {
            get
            {
                return new MvxRelayCommand(() => RequestNavigate<BookListViewModel>(new {category = ListNameEncoded}));
            }
        }
    }
}