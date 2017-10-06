using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxMasterDetailPagePresentationAttribute : MvxPagePresentationAttribute
    {
        public MvxMasterDetailPagePresentationAttribute(MasterDetailPosition position = MasterDetailPosition.Detail)
        {
            Position = position;
        }

        public MasterDetailPosition Position { get; set; } = MasterDetailPosition.Detail;
    }

    public enum MasterDetailPosition
    {
        Master,
        Detail
    }
}
