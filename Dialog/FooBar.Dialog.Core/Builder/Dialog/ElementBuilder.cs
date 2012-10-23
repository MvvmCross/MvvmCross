using System;
using System.Collections.Generic;
using System.Linq;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;

namespace Foobar.Dialog.Core.Builder
{
    public abstract class ElementBuilder : BaseKeyedUserInterfaceBuilder<IElement>
    {
        public const string StandardConventionalEnding = "Element";
        public const string StandardDefaultElementKey = "String";

        protected ElementBuilder(string platformName)
            : base(platformName, StandardConventionalEnding, StandardDefaultElementKey)
        {
        }

        public IElement Build(ElementDescription description)
        {
            var instance = base.Build(description);
            if (instance == null)
            {
                return null;
            }

            FillGroup(instance, description.Group);
            FillProperties(instance, description.Properties);
            FillSections(instance, description.Sections);

            return instance as IElement;
        }

        protected abstract ISection CreateNewSection(SectionDescription sectionDescription);
        protected abstract IGroup CreateNewGroup(GroupDescription groupDescription);

        private ISection Build(SectionDescription sectionDescription)
        {
            if (!CheckDescription(sectionDescription))
            {
                return null;
            }

            var section = CreateNewSection(sectionDescription);

            section.HeaderView = Build(sectionDescription.HeaderElement);
            section.FooterView = Build(sectionDescription.FooterElement);

            FillProperties(section, sectionDescription.Properties);
            FillElements(section, sectionDescription.Elements);

            return section;
        }

        private void FillGroup(object element, GroupDescription groupDescription)
        {
            if (groupDescription == null)
                return;

            var rootElement = element as IRootElement;
            if (rootElement == null)
            {
                throw new ArgumentException("You cannot set a group on an Element of type " + element.GetType().Name);
            }

            if (!CheckDescription(groupDescription))
            {
                return;
            }

            var group = CreateNewGroup(groupDescription);
            FillProperties(group, groupDescription.Properties);
            rootElement.Group = group;
        }

        private void FillElements(ISection section, IEnumerable<ElementDescription> elementDescriptions)
        {
            if (elementDescriptions != null)
            {
                foreach (var elementDescription in elementDescriptions)
                {
                    var element = Build(elementDescription);
                    if (element != null)
                    {
                        section.Add(element);
                    }
                }
            }
        }

        private void FillSections(object instance, IEnumerable<SectionDescription> sectionDescriptions)
        {
            if (sectionDescriptions != null)
            {
                if (instance is IRootElement)
                {
                    var root = instance as IRootElement;
                    foreach (var sectionDescription in sectionDescriptions)
                    {
                        var section = Build(sectionDescription);
                        if (section != null)
                        {
                            root.Add(section);
                        }
                    }
                }
                else if (sectionDescriptions.Any())
                {
                    throw new ArgumentException("Sections cannot be added to Element " + instance.GetType().Name);
                }
            }
        }
    }
}