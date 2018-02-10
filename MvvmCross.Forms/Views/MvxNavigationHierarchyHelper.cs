// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public static class MvxNavigationHierarchyHelper
    {
        public static string Hierarchy(this Application application)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Application Root");
            application.MainPage.PrintHierarchy(sb, null);
            return sb.ToString();
        }

        private static void PrintHierarchy(this Page page, StringBuilder sb, string prefix)
        {
            var isMainPage = prefix == null;
            if (page == null)
            {
                sb.AppendLine(prefix + " (null) ");
                return;
            }

            sb.AppendLine($"{prefix} {page.GetType().Name}({page.NavigationPageType()})");
            prefix = new string(' ', (prefix + page.GetType().Name).Length);

            if (page is MasterDetailPage masterDetail)
            {
                masterDetail.Master.PrintHierarchy(sb, prefix + "Master:");
                masterDetail.Detail.PrintHierarchy(sb, prefix + "Detail:");
            }

            if (page is MultiPage<Page> multiPage)
            {
                for (int i = 0; i < multiPage.Children.Count; i++)
                {
                    var child = multiPage.Children[i];
                    child.PrintHierarchy(sb, prefix + $"[{i}]:");
                }
            }

            if (page is NavigationPage navPage)
            {
                for (int i = 0; i < navPage.Pages.Count(); i++)
                {
                    var child = navPage.Pages.Skip(i).FirstOrDefault();
                    child.PrintHierarchy(sb, prefix + $"[{i}]:");
                }
            }

            if (isMainPage)
            {
                sb.AppendLine("Modals");
                prefix = new string(' ', ("Modals").Length);
                for (int i = 0; i < page.Navigation.ModalStack.Count(); i++)
                {
                    var modal = page.Navigation.ModalStack[i];
                    modal.PrintHierarchy(sb, prefix + $"[{i}]:");
                }
            }
        }

        private static string NavigationPageType(this Page page)
        {
            if (page is MasterDetailPage) return "Master-Detail";
            if (page is TabbedPage) return "Tabbed";
            if (page is CarouselPage) return "Carousel";
            if (page is NavigationPage) return "Navigation";
            return "Content";
        }
    }
}
