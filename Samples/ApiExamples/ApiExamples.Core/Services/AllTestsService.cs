using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;

namespace ApiExamples.Core.ViewModels
{
    public class AllTestsService
        : IAllTestsService
    {
        public AllTestsService()
        {
            All = GetType().GetTypeInfo().Assembly.CreatableTypes()
                           .Where(t => typeof (TestViewModel).IsAssignableFrom(t))
                           .ToList();
        }

        public Type NextViewModelType(TestViewModel currentViewModel)
        {
            var index = All.IndexOf(currentViewModel.GetType());
            var nextIndex = index + 1;
            if (nextIndex < All.Count)
                return All[nextIndex];

            return null;
        }

        public IList<Type> All { get; private set; }
    }
}