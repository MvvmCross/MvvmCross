using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MonoCross.Navigation;

namespace Droid.Container
{
    public interface IDroidView
    {
        Type ActivityType { get; set; }
    }
    public class DroidView<T> : MXView<T>, IDroidView
    {
        public Type ActivityType { get; protected set; }
        public DroidView(Type activityType)
        {
            this.ActivityType = activityType;
        }           
    }
}