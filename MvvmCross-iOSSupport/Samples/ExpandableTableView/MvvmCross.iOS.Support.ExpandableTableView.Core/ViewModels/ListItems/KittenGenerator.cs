using System;
using System.Collections.Generic;

namespace MvvmCross.iOS.Support.ExpandableTableView.Core
{
	public class KittenGenerator
	{
		private readonly List<string> _typesOfKittens = new List<string>
		{
			"Fluffy",
			"ReallyFluffy",
			"Scrappy",
			"Cute",
			"Aggressive",
			"Funny",
			"Bald",
			"WillBite",
			"HasBigPaws"
		};

		private readonly List<string> _names = new List<string>
			{
				"Tiddles",
				"Amazon",
				"Pepsi",
				"Solomon",
				"Butler",
				"Snoopy",
				"Harry",
				"Holly",
				"Paws",
				"Polly",
				"Dixie",
				"Fern",
				"Cousteau",
				"Frankenstein",
				"Bazooka",
				"Casanova",
				"Fudge",
				"Comet"
			};

		private readonly Random _random = new Random();

		public Kitten CreateNewKitten()
		{
			return new Kitten
			{
				Name = _names[_random.Next(_names.Count)],
				ImageUrl = string.Format("http://placekitten.com/{0}/{0}", _random.Next(20) + 300)
			};
		}

		public KittenGroup CreateNewKittenGroup(int numberOfKittens)
		{
			var kittenList = new List<Kitten>();
			for (int x = 0; x < numberOfKittens; x++)
			{
				kittenList.Add(CreateNewKitten());
			}

			return new KittenGroup(kittenList)
			{
				Title = _typesOfKittens[_random.Next(_typesOfKittens.Count)]
			};
		}
	}
}