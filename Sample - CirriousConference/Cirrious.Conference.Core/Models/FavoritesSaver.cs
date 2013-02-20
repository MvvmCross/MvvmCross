using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Json;

namespace Cirrious.Conference.Core.Models
{
    public class FavoritesSaver
        : IMvxServiceConsumer
    {
        private List<string> _toSave;

        public void RequestAsyncSave(List<string> toSave)
        {
            lock (this)
            {
                var wasNull = _toSave == null;
                _toSave = toSave;
                if (wasNull)
                    MvxAsyncDispatcher.BeginAsync(DoSave);
            }
        }

        private void DoSave()
        {
            try
            {
                List<string> toSave;
                lock (this)
                {
                    toSave = _toSave;
                    _toSave = null;
                }

                if (toSave == null)
                    return; // nothing to do

                var jsonConvert = this.GetService<IMvxJsonConverter>();
                var json = jsonConvert.SerializeObject(toSave);
                var fileService = this.GetService<IMvxFileStore>();
                fileService.WriteFile(Constants.FavoritesFileName, json);
            }
            catch (Exception exception)
            {
                Trace.Error("Failed to save favorites: {0}", exception.ToLongString());
            }
        }
    }
}