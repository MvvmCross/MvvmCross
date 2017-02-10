namespace Example.Core.Model
{
    public class MultiItemTemplateModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public SampleItemType Type { get; set; }
    }

    public enum SampleItemType
    {
        TitleItem,
        DescriptionItem
    }
}
