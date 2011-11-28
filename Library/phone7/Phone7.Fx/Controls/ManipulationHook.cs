using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace Phone7.Fx.Controls
{
    public class ManipulationHook
    {
        public delegate void OnManipulationDeltaHandler(ManipulationDeltaEventArgs e);
        public delegate void OnManipulationCompletedHandler(ManipulationCompletedEventArgs e);

        private UIElement _source;
        private bool _is_thumb;
        private bool _is_button;
        private OnManipulationDeltaHandler _delta;
        private OnManipulationCompletedHandler _completed;

        public bool Hook(UIElement source)
        {
            // clear existing hooks
            UnHook();

            // hook this source
            _source = source;

            // derived from type ?
            _is_button = IsSubClassOf(_source, typeof(ButtonBase));
            _is_thumb = IsSubClassOf(_source, typeof(Thumb));

            return true;
        }

        public bool HookDeltaHandler(OnManipulationDeltaHandler callback)
        {
            if (null == _source) return false;
            if (null != _delta) return false;

            _delta = callback;
            _source.ManipulationDelta += ManipulationDelta;

            return true;
        }

        public bool HookCompletedHandler(OnManipulationCompletedHandler callback)
        {
            if (null == _source) return false;
            if (null != _completed) return false;

            _completed = callback;
            _source.ManipulationCompleted += ManipulationCompleted;
            return true;
        }

        public bool UnHook()
        {
            if (null != _source)
            {
                if (null != _delta) _source.ManipulationDelta -= ManipulationDelta;
                if (null != _completed) _source.ManipulationCompleted -= ManipulationCompleted;
            }

            _source = null;
            _delta = null;
            _completed = null;
            return true;
        }

        private bool IsSubClassOf(UIElement ui, Type type)
        {
            while (null != ui)
            {
                if ((ui.GetType() == type) ||
                    (ui.GetType().IsSubclassOf(type)))
                {
                    return true;
                }
                ui = VisualTreeHelper.GetParent(ui) as UIElement;
            }
            return false;
        }

        private void ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // ignore delta event for Thumb
            // and derived.
            if (!_is_thumb)
            {
                // forward manipulation event
                _delta(e);
            }
        }

        private void ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            // mark event as handled for ButtonBase
            // and derived. this will disable clicks.
            if (_is_button)
            {
                e.Handled = true;

                // forward manipulation event
                // as the above call will also
                // cancel event bubbling
                _completed(e);
            }

            // unhook manipulation events
            UnHook();
        }
    }
}
