// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android;

namespace MvvmCross.Platforms.Android.Binding.ResourceHelpers
{
    public class MvxAndroidBindingResource : IMvxAndroidBindingResource
    {
        public int BindingTagUnique => Resource.Id.MvxBindingTagUnique;

        public int[] BindingStylableGroupId => Resource.Styleable.MvxBinding;
        public int BindingBindId => Resource.Styleable.MvxBinding_MvxBind;
        public int BindingLangId => Resource.Styleable.MvxBinding_MvxLang;

        public int[] ControlStylableGroupId => Resource.Styleable.MvxControl;
        public int TemplateId => Resource.Styleable.MvxControl_MvxTemplate;

        public int[] ListViewStylableGroupId => Resource.Styleable.MvxListView;
        public int ListItemTemplateId => Resource.Styleable.MvxListView_MvxItemTemplate;
        public int DropDownListItemTemplateId => Resource.Styleable.MvxListView_MvxDropDownItemTemplate;

        public int[] ExpandableListViewStylableGroupId => Resource.Styleable.MvxExpandableListView;
        public int GroupItemTemplateId => Resource.Styleable.MvxExpandableListView_MvxGroupItemTemplate;
    }
}
