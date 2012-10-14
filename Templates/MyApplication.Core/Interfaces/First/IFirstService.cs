using System;
using System.Collections.Generic;

namespace MyApplication.Core.Interfaces.First
{
    public interface IFirstService
    {
        void GetItems(string key, Action<List<SimpleItem>> onSuccess, Action<FirstServiceError> onError);
    }
}
