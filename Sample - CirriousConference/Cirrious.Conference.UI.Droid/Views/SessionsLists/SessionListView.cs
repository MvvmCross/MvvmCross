using System;
using Android.App;
using Cirrious.Conference.Core.ViewModels.SessionLists;

namespace Cirrious.Conference.UI.Droid.Views.SessionsLists
{
    [Activity]
    public class SessionListView 
        : BaseSessionListView<SessionListViewModel, DateTime>
    {
    }
}