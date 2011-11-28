using System;

namespace Cirrious.MvvmCross.Views
{
    public class MxvViewModelAction
    {
        public string ActionName { get; set; }
        public Type ViewModelType { get; set; }
        public string Key { get { return string.Format("{0}:{1}", ViewModelType.FullName, ActionName); } }

        public MxvViewModelAction()
        {            
        }

        public MxvViewModelAction(Type viewModelType, string actionName)
        {
            ViewModelType = viewModelType;
            ActionName = actionName;
        }
    }

    public class MxvViewModelAction<TViewModel> : MxvViewModelAction
    {
        public MxvViewModelAction(string actionName)
            : base(typeof(TViewModel), actionName)
        {
        }
    }
}