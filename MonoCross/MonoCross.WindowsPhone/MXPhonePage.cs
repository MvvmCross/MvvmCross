using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Linq;

using Microsoft.Phone.Controls;

using MonoCross.Navigation;

namespace MonoCross.WindowsPhone
{
    public abstract class MXPhonePage<T>: PhoneApplicationPage, IMXView
    {
        public virtual void Render() { }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public void SetModel(object model)
        {
            Model = (T)model;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // check if we've been here before!
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                // fetch the model before rendering!!!
                Type t = typeof(T);
                object model = null;
                if (MXPhoneContainer.PhoneContainerInstance.TryGetViewModel(t, out model))
                {
                    SetModel(model);
                }
                else
                {
#warning Need to sort out the navigation stack!
                    throw new Exception("THIS LOOKS TOTALLY BROKEN!");
                    /*
                    var mapping = MXContainer.Instance.App.NavigationMap.FirstOrDefault(layer => layer.Controller.ModelType == t);
                    if (mapping == null)
                    {
                        throw new Exception("The navigation map does not contain any controllers for type " + t.ToString());
                    }

                    mapping.Controller.Load(new Dictionary<string, string>());
                    SetModel(mapping.Controller.GetModel());
                    */
                }

                Render();
            }
        }
    }
}
