using System;
using NUnit.Framework;
using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Views;
using Cirrious.CrossCore;

namespace Cirrious.MvvmCross.Test.ViewModels
{
	[TestFixture]
	public class MvxObservableCollectionTest 
		: MvxIoCSupportingTest
	{
        private class MockMainThreadDispatcher
            : MvxMainThreadDispatcher
            , IMvxViewDispatcher
        {
            public bool RequestMainThreadAction(Action action)
            {
                action();
                return true;
            }

            public bool ShowViewModel(MvxViewModelRequest request)
            {
                throw new NotSupportedException();
            }

            public bool ChangePresentation(MvxPresentationHint hint)
            {
                throw new NotSupportedException();
            }
        }

        protected override void AdditionalSetup()
        {
            Mvx.RegisterSingleton<IMvxViewDispatcher>(new MockMainThreadDispatcher());
        }

		[Test]
		public void Test_CollectionChangedFromMainThread()
		{
            ClearAll();

			var collection = new MvxObservableCollection<string>();
			var callbackCount = 0;
			collection.CollectionChanged += (s, args) => callbackCount++;
			
			collection.Add("value");
			
			Assert.AreEqual(1, callbackCount);
		}
		
		[Test]
		public void Test_CollectionChangedNotForMainThread()
		{
            ClearAll();
            
            var collection = new MvxObservableCollection<string>();
			var callbackCount = 0;
			
			collection.CollectionChanged += (s, args) => callbackCount++;
			
			var task = Task.Factory.StartNew(() =>
			{
				Thread.Sleep(10);
				collection.Add("value");
			});

			Assert.AreEqual(0, callbackCount);

			task.Wait();

			Assert.AreEqual(1, callbackCount);
		}
		
		[Test]
		public void Test_AddRangeRaisedOnce()
		{
            ClearAll();

			var collection = new MvxObservableCollection<string>();
			var callbackCount = 0;
			collection.CollectionChanged += (s, args) => callbackCount++;
			
			collection.AddRange(new string[] {"1", "2"});
			
			Assert.AreEqual(1, callbackCount);
		}	
	}
}

