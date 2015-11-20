// MvxPhonePage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Microsoft.Phone.Controls;
using System.Linq;
using System.Windows.Navigation;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public abstract class MvxPhonePage
        : PhoneApplicationPage
          , IMvxPhoneView
    {
        #region IMvxPhoneView Members

        public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel)DataContext; }
            set { DataContext = value; }
        }

        public void ClearBackStack()
        {
            // note - we do *not* use CanGoBack here - as that seems to always returns true!
            while (NavigationService.BackStack.Any())
                NavigationService.RemoveBackEntry();
        }

        #endregion IMvxPhoneView Members

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var savedState = LoadStateBundle(e);
            this.OnViewCreate(e.Uri, savedState);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var bundle = this.CreateSaveStateBundle();
            SaveStateBundle(e, bundle);

            base.OnNavigatedFrom(e);
        }

        protected virtual IMvxBundle LoadStateBundle(NavigationEventArgs navigationEventArgs)
        {
            // nothing loaded by default
            return null;
        }

        protected virtual void SaveStateBundle(NavigationEventArgs navigationEventArgs, IMvxBundle bundle)
        {
            // not saved by default
        }

        protected override void OnRemovedFromJournal(JournalEntryRemovedEventArgs e)
        {
            base.OnRemovedFromJournal(e);

            this.OnViewDestroy();
        }
    }

    public abstract class MvxPhonePage<TViewModel>
        : MvxPhonePage
        , IMvxPhoneView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}