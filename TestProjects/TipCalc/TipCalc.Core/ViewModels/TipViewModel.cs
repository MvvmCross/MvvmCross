using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using TipCalc.Core.Services;

namespace TipCalc.Core.ViewModels
{
    class TipViewModel : MvxViewModel
    {
        private readonly ICalculationService _calculationService;
        private double _subTotal;
        private int _generosity;

        public TipViewModel(ICalculationService calculation)
        {
            _calculationService = calculation;

            _subTotal = 100;
            _generosity = 10;
            Recalcuate();
        }

        private void Recalcuate()
        {
            Tip = _calculationService.TipAmount(SubTotal, Generosity);
        }

        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override void Start()
        {
            base.Start();
        }

        public double SubTotal
        {
            get { return _subTotal; }
            set
            {
                _subTotal = value;
                RaisePropertyChanged(() => SubTotal);
                Recalcuate();
            }
        }

        public int Generosity
        {
            get { return _generosity; }
            set
            {
                _generosity = value;
                RaisePropertyChanged(() => Generosity);
                Recalcuate();
            }
        }

        double _tip;

        public double Tip
        {
            get { return _tip; }
            set
            {
                _tip = value;
                RaisePropertyChanged(() => Tip);
            }
        }


    }
}
