using System.Collections.ObjectModel;
using Example.Core.Model;
using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class RecyclerViewMultiItemTemplateViewModel : MvxViewModel
    {

        public RecyclerViewMultiItemTemplateViewModel()
        {
            Items = new ObservableCollection<MultiItemTemplateModel>();

            for (int i = 0; i < 50; ++i)
                Items.Add(new MultiItemTemplateModel()
                {
                    Title = "Title " + i,
                    Description = "Description " + i,
                    Type = SampleItemType.TitleItem
                });

            for (int i = 50; i <= 100; ++i)
                Items.Add(new MultiItemTemplateModel()
                {
                    Title = "Title " + i,
                    Description = "Description " + i,
                    Type = SampleItemType.DescriptionItem
                });
        }

        public ObservableCollection<MultiItemTemplateModel> Items { get; }
    }
}
