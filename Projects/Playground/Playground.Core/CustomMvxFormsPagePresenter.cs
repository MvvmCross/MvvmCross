using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public class CustomMvxFormsPagePresenter : MvxFormsPagePresenter
    {
        public CustomMvxFormsPagePresenter(IMvxFormsViewPresenter platformPresenter) : base(platformPresenter)
        {
        }

        public override async Task<bool> ShowContentPage(
            Type view,
            MvxContentPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var showPlatformViews = true;
            // TODO: if we need to do this for tab nav children, need to find a better way other than hard-coding the types..
            if (((MvxViewModelInstanceRequest) request)?.ViewModelInstance?.GetType()
                ?.Equals(typeof(ViewModels.ChildViewModel)) ?? false)
            {
                // this avoids extra tabs being created (on Android.. )
                //TODO: iOS?
                showPlatformViews = false;
            }
            var page = await CloseAndCreatePage(view, request, attribute, showPlatformViews: showPlatformViews);
            await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
            return true;
        }

        private TabbedPage FindTabbedPage(Page page)
        {
            switch (page)
            {
                case TabbedPage tabbedPage:
                    return tabbedPage;
                case NavigationPage navPage:
                    return FindTabbedPage(navPage.CurrentPage);
                default:
                    return null;
            }
        }

        public override async Task PushOrReplacePage(Page rootPage, Page page, MvxPagePresentationAttribute attribute)
        {
            // Make sure we always have a rootPage
            if (rootPage == null)
            {
                rootPage = FormsApplication.MainPage;
            }

            var navigationRootPage = GetPageOfType<NavigationPage>(rootPage);

            // FIX: if our root page is a tabbed page then we need to check the different tabs for the appropriate navigation stack
            if (attribute.WrapInNavigationPage && attribute.HostViewModelType != null)
            {
                var tabbedPage = FindTabbedPage(rootPage);
                if (tabbedPage != null)
                {
                    // find the child that is a navigation stack with a view using the HostViewModelType as its view model
                    foreach (var tab in tabbedPage.Children)
                    {
                        if (!(tab is NavigationPage navPage)) continue;

                        // does this stack have the view with the view model type we want
                        if (navPage?.Navigation?.NavigationStack?.OfType<IMvxPage>().FirstOrDefault(x => x.ViewModel.GetType() == attribute.HostViewModelType) is Page)
                        {
                            navigationRootPage = navPage;
                            break;
                        }
                    }
                }
            }
            // END OF: FIX

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

                // FIX: when attribute indicates page should be wrapped in a NavigationPage, do so
                if (attribute.WrapInNavigationPage)
                {
                    page = CreateNavigationPage(page).Build(tp =>
                    {
                        tp.Title = page.Title;
                        //tp.IconImageSource = page.IconImageSource;
                        tp.Icon = page.Icon;
                    });
                }
                // END OF: FIX

                tabHost.Children.Add(page);
            }
            return true;
        }
    }
}
