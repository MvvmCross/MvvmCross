// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platform.Android.Binding.ResourceHelpers
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
