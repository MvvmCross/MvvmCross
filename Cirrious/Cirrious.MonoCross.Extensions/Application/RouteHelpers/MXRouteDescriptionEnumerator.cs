#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXRouteDescriptionEnumerator.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

#region using

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;

#endregion

namespace Cirrious.MonoCross.Extensions.Application.RouteHelpers
{
    public class MXRouteDescriptionEnumerator : IEnumerator<MXRouteDescription>
    {
        private readonly IMXRouteDescriptionGenerator _patternGenerator;
        private readonly IMXControllerConvention _controllerConvention;
        private readonly IMXActionConvention _actionConvention;

        private List<IMXParameterConvention> _activeParametersList;
        private List<IMXParameterConvention> _defaultParametersList;

        private bool _started;
        private bool _ended;
        private MXRouteDescription _currentDescription;

        public MXRouteDescriptionEnumerator(IMXRouteDescriptionGenerator patternGenerator,
                                            IMXControllerConvention controllerConvention,
                                            IMXActionConvention actionConvention)
        {
            _patternGenerator = patternGenerator;
            _controllerConvention = controllerConvention;
            _actionConvention = actionConvention;

            Reset();
        }

        public void Dispose()
        {
            // deliberately ignored - nothing to dispose
        }

        public bool MoveNext()
        {
            if (!TryMoveNext())
                return false;

            _currentDescription = _patternGenerator.Generate(_controllerConvention, _actionConvention,
                                                             _activeParametersList, _defaultParametersList);

            return true;
        }

        private bool TryMoveNext()
        {
            if (_ended)
                return false;

            if (!_started)
            {
                // this enumeration always has at least one entry
                _started = true;
                return true;
            }

            // is there an optional parameter available to switch from the active to the default list?
            var nextParameterToPop = _activeParametersList.LastOrDefault();
            if (nextParameterToPop == null || !nextParameterToPop.IsOptional)
            {
                // nothing available - game over
                _ended = true;
                return false;
            }

            // do the switch
            _activeParametersList.RemoveAt(_activeParametersList.Count - 1);
            _defaultParametersList.Add(nextParameterToPop);

            return true;
        }

        public void Reset()
        {
            _activeParametersList = _actionConvention.Parameters.ToList();
            _defaultParametersList = new List<IMXParameterConvention>();
            _started = false;
            _ended = false;
            _currentDescription = null;
        }

        // note - technically this should check _ended, but for speed we don't worry
        public MXRouteDescription Current
        {
            get { return _currentDescription; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}