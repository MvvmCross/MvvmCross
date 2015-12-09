namespace MvvmCross.Binding.Droid.ResourceHelpers
{
    public interface IMvxAndroidBindingResource
    {
        int BindingTagUnique { get; }
        int[] BindingStylableGroupId { get; }
        int BindingBindId { get; }
        int BindingLangId { get; }
        int[] ControlStylableGroupId { get; }
        int TemplateId { get; }
        int[] ImageViewStylableGroupId { get; }
        int SourceBindId { get; }
        int[] ListViewStylableGroupId { get; }
        int ListItemTemplateId { get; }
        int DropDownListItemTemplateId { get; }
        int[] ExpandableListViewStylableGroupId { get; }
        int GroupItemTemplateId { get; }
    }
}