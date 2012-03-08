using System;
using System.ComponentModel;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Dialog.Touch.Simple;

namespace SimpleBindingDialog
{
    public class TipViewModel
        : INotifyPropertyChanged
    {
        private float _tipValue;
        public float TipValue
        {
            get { return _tipValue; }
            private set { _tipValue = value; FirePropertyChanged("TipValue"); }
        }

        private float _total;
        public float Total
        {
            get { return _total; }
            private set { _total = value; FirePropertyChanged("Total"); }
        }

        private float _subTotal;
        public float SubTotal
        {
            get { return _subTotal; }
            set { _subTotal = value; FirePropertyChanged("SubTotal"); Recalculate(); }
        }

        private int _tipPercent;
        public int TipPercent
        {
            get { return _tipPercent; }
            set { _tipPercent = value; FirePropertyChanged("TipPercent"); Recalculate(); }
        }

        public TipViewModel()
        {
            SubTotal = 60.0f;
            TipPercent = 12;
            Recalculate();
        }

        private void Recalculate()
        {
            TipValue = ((int)Math.Round(SubTotal * TipPercent)) / 100.0f;
            Total = TipValue + SubTotal;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string whichProperty)
        {
            // take a copy - see RoadWarrior's answer on http://stackoverflow.com/questions/282653/checking-for-null-before-event-dispatching-thread-safe/282741#282741
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(whichProperty));
        }

        #endregion
    }

    public class TipView : MvxSimpleTouchDialogViewController
    {
        public TipView ()
        {
            ViewModel = new TipViewModel();
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.Root = new RootElement("Tip Calc")
                            {
                                new Section("Enter Values")
                                    {
                                        Bind(
                                            new EntryElement("SubTotal", "SubTotal"),
                                            "{'Value':{'Path':'SubTotal','Converter':'Float','Mode':'TwoWay'}}"),
                                        Bind(
                                            new EntryElement("TipPercent", "TipPercent"),
                                            "{'Value':{'Path':'TipPercent','Converter':'Int','Mode':'TwoWay'}}"),
                                        Bind(
                                            new FloatElement(null, null, 0.0f)
                                                {
                                                    ShowCaption = false,
                                                    MinValue = 0.0f,
                                                    MaxValue = 100.0f
                                                },
                                            "{'Value':{'Path':'TipPercent','Converter':'IntToFloat','Mode':'TwoWay'}}"),
                                    },
                                new Section("See the results")
                                    {
                                        Bind(
                                            new StringElement("TipValue"),
                                            "{'Value':{'Path':'TipValue'}}"),
                                        Bind(
                                            new StringElement("Total"), 
                                            "{'Value':{'Path':'Total'}}"),
                                    },
                            };
        }
    }
}

