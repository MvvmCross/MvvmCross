using System.Diagnostics.CodeAnalysis;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Mocks.ViewModels
{
    public class ViewModelMock<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T> where T : MvxViewModel
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
