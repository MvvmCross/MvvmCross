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
                Mvx.Trace(
                    "Could not resolve IMvxJsonConverter, it is going to be hard to initialize with custom object");
                return;
            }

            var deserialized = serializer.DeserializeObject<TInit>(parameter);
            await Init(deserialized);
        }

        protected abstract Task Init(TInit parameter);
    }
}