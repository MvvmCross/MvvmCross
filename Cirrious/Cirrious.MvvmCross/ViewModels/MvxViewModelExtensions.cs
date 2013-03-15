using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
    public static class MvxViewModelExtensions
    {
        public static void CallBundleMethods(this IMvxViewModel viewModel, string methodName, IMvxBundle bundle)
        {
            var methods = viewModel
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == methodName)
                .Where(m => !m.IsAbstract)
                .ToList();

            foreach (var methodInfo in methods)
            {
                viewModel.CallBundleMethod(methodInfo, bundle);
            }
        }

        public static void CallBundleMethod(this IMvxViewModel viewModel, MethodInfo methodInfo, IMvxBundle bundle)
        {
            var parameters = methodInfo.GetParameters().ToArray();
            if (parameters.Count() == 1
                && parameters[0].ParameterType == typeof (IMvxBundle))
            {
                // this method is the 'normal' interface method
                // - we'll call it conventionally outside of this mechanism
                // - so return
                return;
            }
            
            if (parameters.Count() == 1
                && !MvxStringToTypeParser.TypeSupported(parameters[0].ParameterType))
            {
                // call method using typed object
                var value = bundle.Read(parameters[0].ParameterType);
                methodInfo.Invoke(viewModel, new[] {value});
                return;
            }

            // call method using named method arguments
            var invokeWith = bundle.CreateArgumentList(viewModel.GetType(), parameters)
                                       .ToArray();
            methodInfo.Invoke(viewModel, invokeWith);
        }
    }
}