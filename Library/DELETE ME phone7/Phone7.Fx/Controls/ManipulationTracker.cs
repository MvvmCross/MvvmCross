using System;
using System.Windows;
using System.Windows.Controls;

namespace Phone7.Fx.Controls
{
    public class ManipulationTracker
    {
        private const double ToleranceX = 24.0;
        private const double ToleranceY = 40.0;
        private readonly Point Tolerance = new Point(ToleranceX, ToleranceY);

        private bool _tracking;
        private bool _canceled;
        private bool _completed;

        private Point _start;
        private Point _delta;

        private Point _limit;
        private bool _fixed;
        private Orientation _orientation;

        public ManipulationTracker()
        {
            _fixed = false;

            _tracking = false;
            _canceled = false;
            _completed = false;
        }

        public ManipulationTracker(Orientation direction, double limit = 0.0) :
            this()
        {
            _limit = new Point(limit, limit);
            _fixed = true;
            _orientation = direction;
        }

        public void StartTracking(Point start)
        {
            _start = start;
        }

        public bool TrackManipulation(Point total)
        {
            // set total translation
            _delta = total;

            // tracking already completed
            if (_canceled || _completed) return false;

            // cancel if tracked invalid orientation
            if (!_tracking && _fixed)
            {
                if ((_orientation == Orientation.Horizontal) &&
                    (Math.Abs(total.Y) > Math.Abs(Tolerance.Y)))
                    _canceled = true;

                if ((_orientation == Orientation.Vertical) &&
                    (Math.Abs(total.X) > Math.Abs(Tolerance.X)))
                    _canceled = true;
            }

            // complete tracking if off-limits
            if ((_limit.X != 0.0) || (_limit.Y != 0.0))
            {
                if ((Math.Abs(total.X) > Math.Abs(_limit.X)))
                    _completed = true;

                if ((Math.Abs(total.Y) > Math.Abs(_limit.Y)))
                    _completed = true;
            }

            // track movement
            if (!_canceled && !_completed)
            {
                if ((Math.Abs(total.X) > Math.Abs(Tolerance.X)) ||
                    (Math.Abs(total.Y) > Math.Abs(Tolerance.Y)))
                    _tracking = true;
            }

            return !_canceled && !_completed && _tracking;
        }

        public void CancelTracking()
        {
            _tracking = false;
            _canceled = true;
        }

        public void CompleteTracking()
        {
            _tracking = false;
            _completed = true;
        }

        public Point Start { get { return _start; } }
        public Point Delta { get { return _delta; } }
        public Point Position
        {
            get { return new Point(_start.X - _delta.X, _start.Y - _delta.Y); }
            set { _start = value; }
        }

        public bool Tracking { get { return _tracking; } }
        public bool Canceled { get { return _canceled; } }
        public bool Completed { get { return _completed; } }
    }
}
