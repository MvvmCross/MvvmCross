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

namespace MonoCross.Droid
{
    public abstract class MXListActivityView<T> : ListActivity, IMXView
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // fetch the model before rendering!!!
            Model = (T)MXDroidContainer.DroidInstance.ViewModels[typeof(T)];

            // render the model within the view
            Render();
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }

        public event ModelEventHandler ViewModelChanged;
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(this.Model); }
    }
}

