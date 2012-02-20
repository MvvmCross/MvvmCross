using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Shell;

namespace Cirrious.MvvmCross.WindowsPhone.Services.Bookmarks
{
    public class MvxWindowsPhoneLiveTileBookmarkLibrarian 
        : IMvxBookmarkLibrarian
          , IMvxServiceConsumer<IMvxWindowsPhoneViewModelRequestTranslator>
    {
        private const string UniqueIdParameterName = "_id";

        public bool HasBookmark(string uniqueName)
        {
            return FindShellTileFor(uniqueName) != null;
        }

        private static ShellTile FindShellTileFor(string uniqueName)
        {
            return ShellTile.ActiveTiles.FirstOrDefault(x =>
                                                            {
                                                                var parsed = MvxUriExtensionMethods.ParseQueryString(x.NavigationUri);
                                                                string uniqueId;
                                                                if (!parsed.TryGetValue(UniqueIdParameterName, out uniqueId))
                                                                    return false;
                                                                return uniqueId == uniqueName;
                                                            });
        }

        public bool AddBookmark(Type viewModelType, string uniqueName, BookmarkMetadata metadata, IDictionary<string, string> navigationArgs) 
        {
            if (HasBookmark(uniqueName))
                return UpdateBookmark(uniqueName, metadata);

            var liveTileData = ToTileData(metadata);

            var navigationUri =
                this.GetService<IMvxWindowsPhoneViewModelRequestTranslator>().GetXamlUriFor(new MvxShowViewModelRequest(viewModelType, navigationArgs, false, MvxRequestedBy.Bookmark));

            // we sneak in an extra parameter here - our unique name
            var navigationUri2 = new Uri(
                string.Format("{0}{1}{2}={3}",
                              navigationUri.ToString(),
                              navigationUri.ToString().Contains("?") ? "&" : "?",
                              UniqueIdParameterName,
                              uniqueName),
                UriKind.Relative);
            ShellTile.Create(navigationUri2, liveTileData);

            return true;
        }

        private static StandardTileData ToTileData(BookmarkMetadata metadata)
        {
            var liveTileData = new StandardTileData
                                   {
                                       BackgroundImage = metadata.BackgroundImageUri,
                                       Title = metadata.Title,
                                       BackTitle = metadata.BackTitle,
                                       BackContent = metadata.BackContent,
                                       BackBackgroundImage = metadata.BackBackgroundImageUri,
                                       Count = metadata.Count
                                   };
            return liveTileData;
        }

        public bool UpdateBookmark(string uniqueName, BookmarkMetadata metadata)
        {
            var tile = FindShellTileFor(uniqueName);
            if (tile == null) 
                return false;
            tile.Update(ToTileData(metadata));
            return true;
        }
    }
}