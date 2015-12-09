// MvxMissingDescriptionExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.AutoView.Auto.Dialog;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Core.Descriptions.Dialog;
using System.Collections;
using System.Reflection;
using System.Windows.Input;

namespace Cirrious.MvvmCross.AutoView.ExtensionMethods
{
    public static class MvxMissingDescriptionExtensionMethods
    {
        public static ElementDescription CreateMissingDialogDescription(this IMvxViewModel viewModel)
        {
            var viewModelType = viewModel.GetType();
            var propertySection = new SectionAuto(header: "Properties");
            var commandSection = new SectionAuto(header: "Commands");
            var auto = new RootAuto(caption: viewModelType.Name)
                {
                    propertySection,
                    commandSection
                };

            foreach (var property in viewModelType.GetRuntimeProperties())
            {
                if (typeof(ICommand).IsAssignableFrom(property.PropertyType))
                {
                    commandSection.Add(new StringAuto(caption: property.Name)
                    {
                        SelectedCommandNameOverride = property.Name
                    });
                }
                else if (typeof(ICollection).IsAssignableFrom(property.PropertyType))
                {
                    propertySection.Add(new StringAuto(caption: property.Name)
                    {
                        BindingExpressionTextOverride = property.Name + ".Count"
                    });
                }
                else
                {
                    propertySection.Add(new StringAuto(caption: property.Name)
                    {
                        BindingExpressionTextOverride = property.Name
                    });
                }
            }

            var description = auto.ToElementDescription();
            return description;
        }
    }
}