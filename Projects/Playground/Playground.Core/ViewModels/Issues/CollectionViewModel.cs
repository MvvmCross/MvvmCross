using System;
using System.Drawing;
using System.Linq;
using MvvmCross.Commands;
using MvvmCross.UI;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class CollectionViewModel : MvxViewModel
    {
        private readonly Random _random;

        public MvxObservableCollection<AnimalViewModel> Animals { get; }
            = new MvxObservableCollection<AnimalViewModel>();

        public MvxCommand<AnimalViewModel> DeleteAnimalCommand { get; }
        public MvxCommand<int> AddAnimalCommand { get; }
        public MvxCommand<AnimalViewModel> MarkFavoriteCommand { get; }

        public CollectionViewModel()
        {
            _random = new Random();

            DeleteAnimalCommand = new MvxCommand<AnimalViewModel>(DoDeleteAnimalCommand);
            AddAnimalCommand = new MvxCommand<int>(DoAddAnimalCommand);
            MarkFavoriteCommand = new MvxCommand<AnimalViewModel>(DoMarkFavoriteCommand);

            DoAddAnimalCommand(40);
        }

        private string[] _catImageUrls = new[]
        {
            "https://loremflickr.com/320/240/cat",
            "https://www.hillspet.com/content/dam/cp-sites/hills/hills-pet/en_us/exported/cat-care/new-pet-parent/images/mother-cat-and-kitten-sleeping.jpg",
            "https://www.hillspet.com/content/dam/cp-sites/hills/hills-pet/en_us/exported/cat-care/new-pet-parent/images/calico-kitten-hiding-under-chair.jpg",
            "https://www.pets4homes.co.uk/images/articles/4295/early-neutering-of-kittens-pros-and-cons-598ddb68021a9.jpg",
            "https://www.pets4homes.co.uk/images/articles/2827/5-popular-flat-faced-cat-breeds-55460d3d0be16.jpg",
            "http://placekitten.com/200/300",
            "https://www1.cbn.com/sites/default/files/styles/video_ratio_16_9/public/kittenas_hdv.jpg",
            "https://www.petmd.com/sites/default/files/petmd-kitten-facts.jpg"
        };

        private string[] _dogImageUrls = new[]
        {
            "https://loremflickr.com/320/240/dog",
            "https://i.imgur.com/KSftE11.jpg",
            "https://www.thekennelclub.org.uk/media/220388/puppy_environment_alison_spiers.jpg",
            "https://lonelyplanetwpnews.imgix.net/2018/08/ice-cream-3.jpg",
            "https://gfp-2a3tnpzj.stackpathdns.com/wp-content/uploads/2016/07/Yorkshire-Terrier-e1540580455806.jpg",
            "https://news.uoguelph.ca/wp-content/uploads/2018/03/puppy.jpg",
            "https://www.fomobones.com/blog/wp-content/uploads/2018/12/puppy-grows-out.jpg"
        };

        private string[] _monkeyImageUrls = new[]
        {
            "https://loremflickr.com/320/240/monkey",
            "https://allthatsinteresting.com/wordpress/wp-content/uploads/2019/04/baby-primate.jpg",
            "https://images.law.com/contrib/content/uploads/sites/403/2018/04/monkey-selfie-Article-201804131946.jpg",
            "https://www.sciencenews.org/sites/default/files/styles/article-main-image-large/public/2015/12/main/blogposts/121615_ht_monkey-lips_free.jpg",
            "https://thefinanser.com/wp-content/uploads/2015/12/6a01053620481c970b01b7c7617a9f970b-600wi.jpg",
            "https://media-cdn.tripadvisor.com/media/photo-s/05/58/0d/9d/cheeky-monkey-safaris.jpg",
            "https://www.longleat.co.uk/api/v2/storage/public/assets/25/monkey-mayhem/monkey-grid-1_original_original.jpg",
            "https://heritagecorridoraletrail.com/wp-content/uploads/2018/01/babymonkey.jpg"
        };

        private string[] _names = new[]
        {
            "Martijn", "Nicolas", "Nick", "Tomasz", "Stuart", "Marc", "Jeremy", "Jonathan", "Maurits", "Kerry",
            "Will", "Garfield", "Chris", "Przemyslaw", "Guillaume", "Trevor", "Mihal", "Sylvain", "Andres",
            "Erik", "Daniel", "Aaron", "Emmanuel", "Iain", "Martin"
        };

        private Color[] _colors = new[]
        {
            Color.Yellow, Color.Green, Color.Blue, Color.Red, Color.Brown,
            Color.Gold, Color.Orange, Color.Purple, Color.Teal, Color.Pink,
            Color.Azure, Color.Crimson, Color.Cyan, Color.Gray, Color.Silver,
            Color.PapayaWhip, Color.Magenta
        };

        private void DoAddAnimalCommand(int count)
        {
            var animals = Enumerable.Range(0, count).Select(_ => CreateRandomAnimal());
            Animals.AddRange(animals);
        }

        private AnimalViewModel CreateRandomAnimal()
        {
            AnimalViewModel animal = null;

            var name = _names[_random.Next(0, _names.Length - 1)];
            var color = _colors[_random.Next(0, _colors.Length - 1)];

            switch (_random.Next(0, 3))
            {
                case 0: // Cat
                    var cat = new CatViewModel
                    {
                        Name = name,
                        ImageUrl = _catImageUrls[_random.Next(0, _catImageUrls.Length - 1)],
                        FavoriteColor = color
                    };

                    animal = cat;
                    break;
                case 1: // Dog
                    var dog = new DogViewModel
                    {
                        Name = name,
                        ImageUrl = _dogImageUrls[_random.Next(0, _dogImageUrls.Length - 1)],
                        FavoriteColor = color
                    };

                    animal = dog;
                    break;
                case 2: // Monkey
                    var monkey = new MonkeyViewModel
                    {
                        Name = name,
                        ImageUrl = _monkeyImageUrls[_random.Next(0, _monkeyImageUrls.Length - 1)],
                        FavoriteColor = color
                    };

                    animal = monkey;
                    break;
            }

            return animal;
        }

        private void DoDeleteAnimalCommand(AnimalViewModel animal)
        {
            Animals.Remove(animal);
        }

        private void DoMarkFavoriteCommand(AnimalViewModel animal)
        {
            animal.Favorite = !animal.Favorite;
        }

        public abstract class AnimalViewModel : MvxNotifyPropertyChanged
        {
            private string _name;
            private string _imageUrl;
            private Color _favoriteColor;
            private bool _favorite;

            public string Name
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }

            public string ImageUrl
            {
                get => _imageUrl;
                set => SetProperty(ref _imageUrl, value);
            }

            public Color FavoriteColor
            {
                get => _favoriteColor;
                set => SetProperty(ref _favoriteColor, value);
            }

            public bool Favorite
            {
                get => _favorite;
                set => SetProperty(ref _favorite, value);
            }
        }

        public class CatViewModel : AnimalViewModel
        {
        }

        public class DogViewModel : AnimalViewModel
        {
        }

        public class MonkeyViewModel : AnimalViewModel
        {
        }
    }
}
