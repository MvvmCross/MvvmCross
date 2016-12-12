using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.ExpandableTableView.Core
{
	public class FirstViewModel : MvxViewModel
	{
		private List<KittenGroup> _kittenGroups;

		public FirstViewModel()
		{
			KittenGroups = CreateKittenGroups(10).ToList();
		}

		public List<KittenGroup> KittenGroups
		{
			get { return _kittenGroups; }
			set { SetProperty(ref _kittenGroups, value); }
		}

		private readonly KittenGenerator _kittenGenerator = new KittenGenerator();
		private readonly Random _random = new Random();

		protected Kitten CreateKitten()
		{
			return _kittenGenerator.CreateNewKitten();
		}

		protected Kitten CreateKittenNamed(string name)
		{
			var kitten = CreateKitten();
			kitten.Name = name;
			return kitten;
		}

		protected IEnumerable<Kitten> CreateKittens(int count)
		{
			for (var i = 0; i < count; i++)
			{
				yield return CreateKitten();
			}
		}

		protected IEnumerable<KittenGroup> CreateKittenGroups(int count)
		{
			for (var i = 0; i < count; i++)
			{
				yield return CreateKittenGroup(_random.Next(1, count));
			}
		}

		protected KittenGroup CreateKittenGroup(int numberOfKittens)
		{
			return _kittenGenerator.CreateNewKittenGroup(numberOfKittens);
		}
	}
}
