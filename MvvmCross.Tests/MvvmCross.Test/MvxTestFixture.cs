using System;

namespace MvvmCross.Test
{
    public class MvxTestFixture : MvxIoCSupportingTest, IDisposable
    {
        public MvxTestFixture()
        {
            Setup();
        }

        ~MvxTestFixture()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) 
            {
                Reset();
            }
        }
    }
}
