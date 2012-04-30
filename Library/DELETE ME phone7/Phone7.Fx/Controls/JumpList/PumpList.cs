using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Phone7.Fx.Controls.JumpList
{
    internal  class PumpList<T>
    {
        public event EventHandler OnPumpCompleted;
        public event EventHandler OnPumping;

        readonly ObservableCollection<T> _items = new ObservableCollection<T>();
        readonly DispatcherTimer _timer = new DispatcherTimer();
        readonly IList<T> _source;

        readonly int _totalPumpItems;
        int _currentPumpItem;
        int _maxPumpItems = 10;

        public ObservableCollection<T> Items
        {
            get { return _items; }
        }

        public int MaxPumpItems
        {
            get { return _maxPumpItems; }
            set { _maxPumpItems = value; }
        }

        public PumpList(IList<T> items)
        {
            _source = items;

            _totalPumpItems = _source.Count;

            // Prefetch the first item
            if (_totalPumpItems > 0)
            {
                Items.Add(_source[0]);
                _currentPumpItem = 1;
            }

            _timer.Tick += TimerTick;
        }

        public void StartPump()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (_currentPumpItem == _totalPumpItems || _currentPumpItem == _maxPumpItems)
            {
                _timer.Stop();

                for (int index = _maxPumpItems; index < _totalPumpItems; index++)
                {
                    Items.Add(_source[index]);
                    if (OnPumping != null)
                        OnPumping(this, EventArgs.Empty);
                }

                if (OnPumpCompleted != null)
                    OnPumpCompleted(this, EventArgs.Empty);

                return;
            }

            Items.Add(_source[_currentPumpItem++]);
            if (OnPumping != null)
                OnPumping(this, EventArgs.Empty);
        }
    }
}