using System.Linq;
using Example.Core;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters;

namespace Example.Droid.Converters
{
    public class ListItemGroupDataConverter : IMvxGroupedDataConverter
    {
        public MvxGroupedData ConvertToMvxGroupedData(object item)
        {
            var groupedListItem = item as IGrouping<string, ListItem>;
            return new MvxGroupedData()
            {
                Items = groupedListItem,
                Key = groupedListItem.Key
            };
        }
    }
}