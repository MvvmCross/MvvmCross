using System.Collections.Generic;
using System.Linq;
using Cirrious.Conference.Core.Models.Raw;
using Cirrious.Conference.Core.ViewModels.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ViewModels
{
    public class BaseSponsorsViewModel
        : BaseConferenceViewModel
    {
        protected void LoadFrom(IEnumerable<Sponsor> source)
        {
            var sponsors = source
                .ToList();
            sponsors.Sort((a, b) => a.Name.CompareTo(b.Name));
            Sponsors = sponsors.Select(x => new WithCommand<Sponsor>(x, new MvxCommand(() => ShowWebPage(x.Url)))).ToList();
        }

        public IList<WithCommand<Sponsor>> Sponsors { get; private set; }        
    }
}