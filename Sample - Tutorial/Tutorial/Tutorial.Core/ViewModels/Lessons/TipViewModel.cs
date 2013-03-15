using System;
using Cirrious.MvvmCross.ViewModels;

namespace Tutorial.Core.ViewModels.Lessons
{
    public class TipViewModel
        : MvxViewModel
    {
        private float _tipValue;
        public float TipValue
        {
            get { return _tipValue; }
            private set { _tipValue = value; RaisePropertyChanged(() => TipValue); }
        }

        private float _total;
        public float Total
        {
            get { return _total; }
            private set { _total = value; RaisePropertyChanged(() => Total); }
        }

        private float _subTotal;
        public float SubTotal
        {
            get { return _subTotal; }
            set { _subTotal = value; RaisePropertyChanged(() => SubTotal); Recalculate(); }
        }

        private int _tipPercent;
        public int TipPercent
        {
            get { return _tipPercent; }
            set { _tipPercent = value; RaisePropertyChanged(() => TipPercent); Recalculate(); }
        }

        public TipViewModel()
        {
            SubTotal = 60.0f;
            TipPercent = 12;
            Recalculate();
        }

        private void Recalculate()
        {
            if (TipPercent > 100)
                TipPercent = 100;
            if (TipPercent < 0)
                TipPercent = 0;

            TipValue = ((int)Math.Round(SubTotal * TipPercent)) / 100.0f;
            Total = TipValue + SubTotal;
        }
    }
}