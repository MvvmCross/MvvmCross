namespace Example.Droid.Converters
{
    public class ListItemGroupDataConverter : IMvxGroupedDataConverter
    {
        public MvxGroupedData ConvertToMvxGroupedData(object item)
        {
            var groupedListItem = item as IGrouping<string, ListItem>;
            return new MvxGroupedData
            {
                Items = groupedListItem,
                Key = groupedListItem.Key
            };
        }
    }
}