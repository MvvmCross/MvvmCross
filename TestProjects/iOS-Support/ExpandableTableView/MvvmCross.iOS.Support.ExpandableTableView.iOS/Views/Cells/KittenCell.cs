using System;
using MvvmCross.Binding.iOS.Views;
using Foundation;

namespace MvvmCross.iOS.Support.ExpandableTableView.iOS
{
    [Register("KittenCell")]
    public partial class KittenCell : MvxTableViewCell
    {
        private const string BindingText = "Name Name;ImageUrl ImageUrl";

        private MvxImageViewLoader _imageHelper;

        public KittenCell()
            : base(BindingText)
        {
            InitialiseImageHelper();
        }

        public KittenCell(IntPtr handle)
            : base(BindingText, handle)
        {
            InitialiseImageHelper();
        }

        public string Name
        {
            get { return MainLabel.Text; }
            set { MainLabel.Text = value; }
        }

        public string ImageUrl
        {
            get { return _imageHelper.ImageUrl; }
            set { _imageHelper.ImageUrl = value; }
        }

        public static float GetCellHeight()
        {
            return 120f;
        }

        private void InitialiseImageHelper()
        {
            _imageHelper = new MvxImageViewLoader(() => KittenImageView);
        }
    }
}