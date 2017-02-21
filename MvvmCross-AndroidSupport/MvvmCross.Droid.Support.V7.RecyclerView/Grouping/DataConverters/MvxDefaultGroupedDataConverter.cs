namespace MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters
{
    internal class MvxDefaultGroupedDataConverter : IMvxGroupedDataConverter
    {
        public MvxGroupedData ConvertToMvxGroupedData(object item) => item as MvxGroupedData;
    }
}