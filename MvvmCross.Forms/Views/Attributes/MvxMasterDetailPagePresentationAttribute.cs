using System;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxMasterDetailPagePresentationAttribute : MvxPagePresentationAttribute
    {
        public MvxMasterDetailPagePresentationAttribute(MasterDetailPosition position = MasterDetailPosition.Detail)
        {
            Position = position;

            // If this page is to be the master, the default behaviour should be that the page is not wrapped
            // in a navigation page. This is not the case for Root or Detail pages where default behaviour
            // would be to support navigation
            if (position == MasterDetailPosition.Master)
            {
                WrapInNavigationPage = false;
            }
        }

        public MasterDetailPosition Position { get; set; } = MasterDetailPosition.Detail;
    }

    public enum MasterDetailPosition
    {
        Root,
        Master,
        Detail
    }
}