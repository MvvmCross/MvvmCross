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
    public abstract class MXTabActivityView<T> : TabActivity, IMXView
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // fetch the model before rendering!!!
            Type t = typeof(T);
            if (MXDroidContainer.DroidInstance.ViewModels.ContainsKey(t))
            {
                SetModel(MXDroidContainer.DroidInstance.ViewModels[t]);
            }
            else
            {
                var mapping = MXContainer.Instance.App.NavigationMap.FirstOrDefault(layer => layer.Controller.ModelType == t);
                if (mapping == null)
                {
                    throw new ApplicationException("The navigation map does not contain any controllers for type " + t.ToString());
                }
                else
                {
                    mapping.Controller.Load(new Dictionary<string, string>());
                    SetModel(mapping.Controller.GetModel());
                }
            }

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

