// AchievementElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Globalization;

namespace CrossUI.Droid.Dialog.Elements
{
    public class AchievementElement : Element
    {
        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                ActOnCurrentAttachedCell(UpdateDescriptionDisplay);
            }
        }

        private int _percentageComplete;

        public int PercentageComplete
        {
            get { return _percentageComplete; }
            set
            {
                _percentageComplete = value;
                ActOnCurrentAttachedCell(UpdatePercentageCompleteDisplay);
            }
        }

        private Bitmap _achievementImage;

        public Bitmap AchievementImage
        {
            get { return _achievementImage; }
            set
            {
                _achievementImage = value;
                ActOnCurrentAttachedCell(UpdateAchievementImageDisplay);
            }
        }

        public string Group;

        public AchievementElement(string caption = null, string description = null, int percentageComplete = 0, Bitmap achievementImage = null)
            : base(caption, "dialog_achievements")
        {
            Description = description;
            PercentageComplete = percentageComplete;
            AchievementImage = achievementImage;
        }

        protected override void UpdateCellDisplay(View cell)
        {
            UpdateDescriptionDisplay(cell);
            UpdateAchievementImageDisplay(cell);
            UpdatePercentageCompleteDisplay(cell);
            base.UpdateCellDisplay(cell);
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            ImageView achivementImage;
            TextView caption;
            TextView description;
            TextView percentageComplete;

            DroidResources.DecodeAchievementsElementLayout(Context, cell, out caption, out description,
                                                           out percentageComplete, out achivementImage);

            if (caption != null)
                caption.Text = Caption;
        }

        protected virtual void UpdateAchievementImageDisplay(View cell)
        {
            if (cell == null)
                return;

            ImageView achivementImage;
            TextView caption;
            TextView description;
            TextView percentageComplete;

            DroidResources.DecodeAchievementsElementLayout(Context, cell, out caption, out description,
                                                           out percentageComplete, out achivementImage);

            if (achivementImage != null)
            {
                if (AchievementImage != null)
                    achivementImage.SetImageBitmap(AchievementImage);
                else
                {
                    // TODO! Should clear the image!
                }
            }
        }

        protected virtual void UpdatePercentageCompleteDisplay(View cell)
        {
            if (cell == null)
                return;

            ImageView achivementImage;
            TextView caption;
            TextView description;
            TextView percentageComplete;

            DroidResources.DecodeAchievementsElementLayout(Context, cell, out caption, out description,
                                                           out percentageComplete, out achivementImage);

            if (percentageComplete != null)
                percentageComplete.Text = PercentageComplete.ToString(CultureInfo.InvariantCulture);
        }

        protected virtual void UpdateDescriptionDisplay(View cell)
        {
            if (cell == null)
                return;

            ImageView achivementImage;
            TextView caption;
            TextView description;
            TextView percentageComplete;

            DroidResources.DecodeAchievementsElementLayout(Context, cell, out caption, out description,
                                                           out percentageComplete, out achivementImage);

            // TODO - this is slow for things which don't need complete rebinding...
            caption.Text = Caption;
            description.Text = Description;
            percentageComplete.Text = PercentageComplete.ToString(CultureInfo.InvariantCulture);
            if (AchievementImage != null)
            {
                achivementImage.SetImageBitmap(AchievementImage);
            }
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            View view = DroidResources.LoadAchievementsElementLayout(context, parent, LayoutName);
            if (view == null)
            {
                Android.Util.Log.Error("AchievementElement", "GetViewImpl failed to load template view");
            }
            return view;
        }
    }
}