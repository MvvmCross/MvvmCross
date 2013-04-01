using System;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Commands;

namespace Tutorial.Core.ViewModels.Lessons
{
	public class CollectionViewModel
		: MvxViewModel
	{
		public MvxObservableCollection<string> Items { get; private set; }

		public CollectionViewModel()
		{
			Items = new MvxObservableCollection<string>(new string[] { "one", "two", "three" });
		}

		public ICommand AddCommand
		{
			get { return new MvxRelayCommand(OnAddCommand); }
		}

		public ICommand RemoveRandom
		{
			get { return new MvxRelayCommand(OnRemoveRandomCommand); }
		}

		private void OnAddCommand()
		{
			// simulate async task
			Task.Factory.StartNew(() => {

				// simulate heavy job
				Thread.Sleep(500);

				// create few items to add
				var count = Items.Count;
				var items = new string[] { count.ToString(), (count + 1).ToString() };

				// fill ObservableCollection (do not use InvokeOnMainThread)
				Items.AddRange(items);
			});
		}

		private void OnRemoveRandomCommand()
		{
			Task.Factory.StartNew(() => {

				Thread.Sleep(500);

				var index = new System.Random().Next(Items.Count);
				Items.RemoveAt(index);
			});
		}
	}
}

