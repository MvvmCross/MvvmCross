using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Mocks.ViewModels
{
    public class ViewModelMock<T> where T : MvxViewModel
    {
        private readonly T _object;

        [RequiresUnreferencedCode("Cannot statically analyze the type of instance so its members may be trimmed")]
        public ViewModelMock()
        {
            _object = (T)Activator.CreateInstance(typeof(T));
            _object.InitializeTask = MvxNotifyTask.Create(() => Task.CompletedTask);
        }

        public T Object
        {
            get
            {
                return _object;
            }
        }
    }
}
