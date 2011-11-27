using System;
using System.Collections.Generic;
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;
using MonoCross.Navigation;
using MonoCross.Navigation.ActionResults;
using Cirrious.MonoCross.Extensions.ExtensionMethods;

namespace Cirrious.MonoCross.Extensions.Controllers.ActionResults
{
    public class MXRedirectToActionResult<T> : MXRedirectActionResultBase where T : IMXConventionBasedController
    {
        private readonly string _actionName;
        private readonly IDictionary<string, string> _parameterValues;

        public MXRedirectToActionResult()
            : this(null, null)
        {
        }

        public MXRedirectToActionResult(string actionName)
            : this(actionName, null)
        {
        }

        public MXRedirectToActionResult(string actionName, object parameterObject)
            : this(actionName, parameterObject.ToSimplePropertyDictionary())
        {
        }

        public MXRedirectToActionResult(string actionName, IDictionary<string, string> parameterValues)
        {
            _actionName = actionName;
            _parameterValues = parameterValues;
        }

        public override void Perform(IMXContainer container, IMXView fromView, IMXController controller)
        {
            MXConventionNavigationExtensionMethods.Navigate<T>(_actionName, _parameterValues);
        }
    }
}
