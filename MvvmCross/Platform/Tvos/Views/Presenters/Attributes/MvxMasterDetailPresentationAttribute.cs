using MvvmCross.Core.Views;
namespace MvvmCross.tvOS.Views.Presenters.Attributes
{
    public class MvxMasterDetailPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = true;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
        public MasterDetailPosition Position { get; set; } = MasterDetailPosition.Detail;

        public MvxMasterDetailPresentationAttribute(MasterDetailPosition position = MasterDetailPosition.Detail)
        {
            Position = position;
        }
    }

    public enum MasterDetailPosition
    {
        Root,
        Master,
        Detail
    }
}
