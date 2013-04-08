using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.ThreadUtils;
using Cirrious.MvvmCross.ViewModels;

namespace Tutorial.Core.ViewModels.Lessons
{
    public class PullToRefreshViewModel
        : MvxViewModel        
    {
        private IMvxThreadSleep Sleeper
        {
            get
            {
                Cirrious.MvvmCross.Plugins.ThreadUtils.PluginLoader.Instance.EnsureLoaded();
                return Mvx.Resolve<IMvxThreadSleep>();
            }
        }
        public class SimpleEmail
        {
            public string From { get; set; }    
            public string Header { get; set; }    
            public string Message { get; set; }    
        }

        private static readonly Random Random = new Random();

        private ObservableCollection<SimpleEmail> _emails;
        public ObservableCollection<SimpleEmail> Emails
        {
            get { return _emails; }
            private set { _emails = value; RaisePropertyChanged(() => Emails); }
        }

        public ICommand RefreshHeadCommand
        {
            get
            {
                return new MvxCommand(DoRefreshHead);
            }
        }

        public ICommand RefreshTailCommand
        {
            get
            {
                return new MvxCommand(DoRefreshTail);
            }
        }

        private void DoRefreshHead()
        {
            if (IsRefreshingHead)
                return;

            IsRefreshingHead = true;
            MvxAsyncDispatcher.BeginAsync(() =>
                                             {
                                                 Sleeper.Sleep(TimeSpan.FromSeconds(2.0));
                                                 this.InvokeOnMainThread(() =>
                                                                             {
                                                                                 AddEmailsHead(1 + Random.Next(5));
                                                                                 IsRefreshingHead = false;
                                                                             });
                                             });
        }

        private void DoRefreshTail()
        {
            if (IsRefreshingTail)
                return;

            IsRefreshingTail = true;
            MvxAsyncDispatcher.BeginAsync(() =>
            {
                Sleeper.Sleep(TimeSpan.FromSeconds(2.0));
                this.InvokeOnMainThread(() =>
                {
                    AddEmailsTail(1 + Random.Next(5));
                    IsRefreshingTail = false;
                });
            });
        }

        private bool _isRefreshingHead;
        public bool IsRefreshingHead
        {
            get { return _isRefreshingHead; }
            private set { _isRefreshingHead = value; RaisePropertyChanged(() => IsRefreshingHead); }
        }

        private bool _isRefreshingTail;
        public bool IsRefreshingTail
        {
            get { return _isRefreshingTail; }
            private set { _isRefreshingTail = value; RaisePropertyChanged(() => IsRefreshingTail); }
        }

        public PullToRefreshViewModel()
        {
            Emails = new ObservableCollection<SimpleEmail>();
            AddEmailsTail(5);
        }

        private void AddEmailsHead(int count)
        {
            for (var i = 0; i < count; i++ )
            {
                Emails.Insert(0, CreateEmail());
            }
        }

        private void AddEmailsTail(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Emails.Add(CreateEmail());
            }
        }

        private static SimpleEmail CreateEmail()
        {
            return new SimpleEmail()
                       {
                           From = PickName(),
                           Header = PickHeader(),
                           Message = GenerateMessage()
                       };
        }

        private static readonly string[] Lorem =
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                .Split(' ');

        private static string GenerateMessage()
        {
            var toReturn = new StringBuilder();
            var length = Random.Next(20) + 20;
            for (var i = 0; i < length; i++)
            {
                toReturn.Append(PickOne(Lorem));
                toReturn.Append(" ");
            }
            return toReturn.ToString();
        }

        private static readonly string[] Headers = new[] { "Would like to meet", "About your blog", "Important message from your bank", "Thought you would be interested in this...", "Borrow up to 1000 with free delivery" };
        private static string PickHeader()
        {
            return PickOne(Headers);
        }

        private static readonly string[] Names = new[] { "Fred", "Barney", "Wilma", "Betty", "Dino" };
        private static string PickName()
        {
            return PickOne(Names);
        }

        private static string PickOne(string[] input)
        {
            return input[Random.Next(input.Length)];
        }
    }
}