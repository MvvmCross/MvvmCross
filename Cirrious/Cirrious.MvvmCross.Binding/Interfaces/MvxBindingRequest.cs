#region Copyright
// <copyright file="MvxBindingRequest.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
namespace Cirrious.MvvmCross.Binding.Interfaces
{
    public class MvxBindingRequest
    {
        public MvxBindingRequest()
        {
        }

        public MvxBindingRequest(object source, object target, MvxBindingDescription description)
        {
            Target = target;
            Source = source;
            Description = description;
        }

        public object Target { get; set; }
        public object Source { get; set; }
        public MvxBindingDescription Description { get; set; }
    }
}