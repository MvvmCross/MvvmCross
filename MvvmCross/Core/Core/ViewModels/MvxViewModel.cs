// MvxViewModel.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using System.Threading.Tasks;

namespace MvvmCross.Core.ViewModels
{
    using MvvmCross.Platform.Exceptions;

    public abstract class MvxViewModel
        : MvxNavigatingObject
          , IMvxViewModel
    {
        protected MvxViewModel()
        {
            this.RequestedBy = MvxRequestedBy.Unknown;
        }
		public MvxRequestedBy RequestedBy { get; set; }

		public virtual void Appearing()
		{
		}

		public virtual void Appeared()
		{
		}

		public virtual void Disappearing()
		{
		}

		public virtual void Disappeared()
		{
		}

        public void Init(IMvxBundle parameters)
        {
            this.InitFromBundle(parameters);
        }

        public void ReloadState(IMvxBundle state)
        {
            this.ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public virtual void Destroy ()
        {
        }

        public void SaveState(IMvxBundle state)
        {
            this.SaveStateToBundle(state);
        }

        protected virtual void InitFromBundle(IMvxBundle parameters)
        {
        }

        protected virtual void ReloadFromBundle(IMvxBundle state)
        {
        }

        protected virtual void SaveStateToBundle(IMvxBundle bundle)
        {
        }
    }

    public abstract class MvxViewModel<TInit> : MvxViewModel, IMvxViewModelInitializer<TInit>
    {
        public async Task Init(string parameter)
        {
            IMvxJsonConverter serializer;
            if (!Mvx.TryResolve(out serializer))
            {
                throw new MvxIoCResolveException("There is no implementation of IMvxJsonConverter registered. You need to use the MvvmCross Json plugin or create your own implementation of IMvxJsonConverter.");
            }

            var deserialized = serializer.DeserializeObject<TInit>(parameter);
            await Init(deserialized);
        }

        protected abstract Task Init(TInit parameter);
    }
}