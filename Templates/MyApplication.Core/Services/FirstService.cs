using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MyApplication.Core.Interfaces.First;

namespace MyApplication.Core.Services
{
    public class FirstService : IFirstService
    {
        public void GetItems(string key, Action<List<SimpleItem>> onSuccess, Action<FirstServiceError> onError)
        {
            ThreadPool.QueueUserWorkItem(ignored =>
                {
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        onError(FirstServiceError.ErrorEmptyKey);
                        return;
                    }

                    var success = new List<SimpleItem>();
                    for (var i = 0; i < 10; i++)
                    {
                        success.Add(new SimpleItem()
                            {
                                Id = i,
                                Title = "Title " + i + "("  + key + ")",
                                Notes = string.Format("This item returned from {0} - here's a GUID: {1}", key, Guid.NewGuid().ToString("N")),
                                When = DateTime.UtcNow.AddMinutes( -i )
                            });
                    }
                    onSuccess(success);
                });
        }
    }
}
