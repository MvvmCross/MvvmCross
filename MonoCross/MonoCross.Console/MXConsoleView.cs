using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;

namespace MonoCross.Console
{
    public class MXConsoleView<T>: IMXView
    {
        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public void SetModel(object model)
        {
            Model = (T)model;
        }

        public event ModelEventHandler ViewModelChanged;
        public virtual void OnViewModelChanged(object model) { }
        public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(this.Model); }

        public void DefaultInputAndNavigate(Dictionary<string, string> inputMap)
        {
            do
            {
                string input = System.Console.ReadLine().Trim();
                if (input.Length == 0 || inputMap == null || inputMap.Count == 0)
                {
                    this.Back();
                    return;
                }

                string action;
                if (inputMap.TryGetValue(input, out action))
                {
                    this.Navigate(action);
                    return;
                }

                System.Console.WriteLine("Invalid input, retry input or Enter to go back");

            } while (true);
        }

        public virtual void Render()
        {
        }
    }
}
