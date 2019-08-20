using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using Xamarin.Forms;

#pragma warning disable

namespace Playground.Core
{
    /// <summary>
    /// Fixes https://github.com/MvvmCross/MvvmCross/issues/2502
    /// </summary>
    /// <seealso cref="MvvmCross.Forms.Presenters.MvxFormsPagePresenter" />
    public class SavingsAppMvxFormsPagePresenter : MvxFormsPagePresenter
    {
        #region Public Constructors

        public SavingsAppMvxFormsPagePresenter(IMvxFormsViewPresenter platformPresenter) : base(platformPresenter)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override async Task PushOrReplacePage(Page rootPage, Page page, MvxPagePresentationAttribute attribute)
        {
            // Make sure we always have a rootPage
            if (rootPage == null)
            {
                rootPage = FormsApplication.MainPage;
            }

            var navigationRootPage = GetPageOfType<NavigationPage>(rootPage);

            // Step down through any nested navigation pages to make sure we're pushing to the
            // most nested navigation page
            if (attribute.WrapInNavigationPage &&
                navigationRootPage?.CurrentPage is NavigationPage navigationNestedPage)
            {
                await PushOrReplacePage(navigationNestedPage, page, attribute);
                return;
            }

            // Handle the case where the page should be wrapped in a navigation page
            if (attribute.WrapInNavigationPage)
            {
                // Look at parent and see whether it's a navigation page,
                // if it is, then use it to navigate to the new page
                if (navigationRootPage == null && rootPage?.Parent is NavigationPage parentNavigation)
                {
                    navigationRootPage = parentNavigation;
                }

                // If the root isn't a navigation page, we need to wrap the new page
                // in a navigation wrapper.
                if (navigationRootPage == null || attribute.NoHistory)
                {
                    var navpage = CreateNavigationPage(page);
                    ReplacePageRoot(rootPage, navpage, attribute);
                }
                else
                {
                    await navigationRootPage.PushAsync(page, attribute.Animated);
                }
            }
            else
            {
                ReplacePageRoot(rootPage, page, attribute);
            }
        }

        public override async Task<bool> ShowTabbedPage(
            Type view,
            MvxTabbedPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var page = await CloseAndCreatePage(view, request, attribute);

            if (attribute.Position == TabbedPosition.Root)
            {
                if (page is TabbedPage tabbedPageRoot)
                {
                    await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(TabbedPage)}");
            }
            else
            {
                var tabHost = GetPageOfType<TabbedPage>();
                if (tabHost == null)
                {
                    //MvxFormsLog.Instance.Trace($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                    tabHost = new TabbedPage();
                    await PushOrReplacePage(FormsApplication.MainPage, tabHost, attribute);
                }

                tabHost.Children.Add(page);
            }
            return true;
        }

        #endregion Public Methods
    }
}
