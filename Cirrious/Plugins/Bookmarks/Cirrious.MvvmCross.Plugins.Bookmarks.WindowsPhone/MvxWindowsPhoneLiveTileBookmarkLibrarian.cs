#region Copyright

// <copyright file="MvxWindowsPhoneLiveTileBookmarkLibrarian.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.ExtensionMethods;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Shell;

namespace Cirrious.MvvmCross.Plugins.Bookmarks.WindowsPhone
{
    public class MvxWindowsPhoneLiveTileBookmarkLibrarian
        : IMvxBookmarkLibrarian
          , IMvxServiceConsumer<IMvxWindowsPhoneViewModelRequestTranslator>
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
                this.GetService()
                    .GetXamlUriFor(new MvxShowViewModelRequest(viewModelType, navigationArgs, false,
                                                               MvxRequestedBy.Bookmark));

            // we sneak in an extra parameter here - our unique name
            var navigationUri2 = new Uri(
                string.Format("{0}{1}{2}={3}",
                              navigationUri,
                              navigationUri.ToString().Contains("?") ? "&" : "?",
                              UniqueIdParameterName,
                              uniqueName),
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

        #endregion

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