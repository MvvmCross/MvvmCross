using System;
using System.Collections.Generic;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using static Playground.Core.ViewModels.CollectionViewModel;

namespace Playground.Droid.TemplateSelectors
{
    public class AnimalTemplateSelector : IMvxTemplateSelector
    {
        private readonly Dictionary<Type, int> _itemsTypeDictionary = new Dictionary<Type, int>
        {
            [typeof(CatViewModel)] = Resource.Layout.itemtemplate_cat,
            [typeof(DogViewModel)] = Resource.Layout.itemtemplate_dog,
            [typeof(MonkeyViewModel)] = Resource.Layout.itemtemplate_monkey,
        };

        public int ItemTemplateId { get; set; }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public int GetItemViewType(object forItemObject)
        {
            return _itemsTypeDictionary[forItemObject.GetType()];
        }
    }
}
