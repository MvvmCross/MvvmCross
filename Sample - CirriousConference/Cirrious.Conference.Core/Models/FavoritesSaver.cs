using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Newtonsoft.Json;

namespace Cirrious.Conference.Core.Models
{
    public class FavoritesSaver
        : IMvxServiceConsumer<IMvxSimpleFileStoreService>
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

                var json = JsonConvert.SerializeObject(toSave);
                var service = this.GetService();
                service.WriteFile(Constants.FavoritesFileName, json);
            }
            catch (Exception exception)
            {
                Trace.Error("Failed to save favorites: {0}", exception.ToLongString());
            }
        }
    }
}