// MvxWindowsPhoneLiveTileBookmarkLibrarian.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.WindowsPhone.Platform;
using MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Plugins.Bookmarks.WindowsPhone
{
    public class MvxWindowsPhoneLiveTileBookmarkLibrarian
        : IMvxBookmarkLibrarian

    {
        private const string UniqueIdParameterName = "_id";

        #region IMvxBookmarkLibrarian Members

        public bool HasBookmark(string uniqueName)
        {
            return FindShellTileFor(uniqueName) != null;
        }

        public bool AddBookmark(Type viewModelType, string uniqueName, MvxBookmarkMetadata metadata,
                                IDictionary<string, string> navigationArgs)
        {
            if (HasBookmark(uniqueName))
                return UpdateBookmark(uniqueName, metadata);

            var liveTileData = ToTileData(metadata);

            var navigationUri =
                Mvx.Resolve<IMvxPhoneViewModelRequestTranslator>()
                    .GetXamlUriFor(new MvxViewModelRequest(viewModelType, new MvxBundle(navigationArgs), null,
                                                               MvxRequestedBy.Bookmark));

            // we sneak in an extra parameter here - our unique name
            var navigationUri2 = new Uri(
                $"{navigationUri}{(navigationUri.ToString().Contains("?") ? "&" : "?")}{UniqueIdParameterName}={uniqueName}",
                UriKind.Relative);
            ShellTile.Create(navigationUri2, liveTileData);

            return true;
        }

        public bool UpdateBookmark(string uniqueName, MvxBookmarkMetadata metadata)
        {
            var tile = FindShellTileFor(uniqueName);
            if (tile == null)
                return false;
            tile.Update(ToTileData(metadata));
            return true;
        }

        #endregion IMvxBookmarkLibrarian Members

        private static ShellTile FindShellTileFor(string uniqueName)
        {
            return ShellTile.ActiveTiles.FirstOrDefault(x =>
                {
                    var parsed = x.NavigationUri.ParseQueryString();
                    string uniqueId;
                    if (!parsed.TryGetValue(UniqueIdParameterName, out uniqueId))
                        return false;
                    return uniqueId == uniqueName;
                });
        }

        private static StandardTileData ToTileData(MvxBookmarkMetadata metadata)
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
    }
}