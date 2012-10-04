using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Platform;
using Windows.ApplicationModel;

namespace Cirrious.MvvmCross.WinRT.Platform
{
    public class MvxWinRTResourceLoader : MvxBaseResourceLoader
    {
        #region Implementation of IMvxResourceLoader


        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            var folder = Package.Current.InstalledLocation;
            var split = resourcePath.Split('/', '\\').ToList();

            while (split.Count > 1)
            {
                var folderName = split.First();
                split.RemoveAt(0);
                folder = folder.GetFolderAsync(folderName).Await();
            }

            var file = folder.GetFileAsync(split.First()).Await();
            var streamWithContent = file.OpenReadAsync().Await();
            var stream = streamWithContent.AsStreamForRead();
            streamAction(stream);
        }


        #endregion
    }
}
