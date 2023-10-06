using System;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Mocks.ViewModels
{
    public class ViewModelMock<T> where T : MvxViewModel
    {
        private readonly T _object;

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
