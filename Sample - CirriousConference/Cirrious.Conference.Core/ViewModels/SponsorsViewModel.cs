using System.Linq;
using System.Collections.Generic;
using Cirrious.Conference.Core.Models;
using Cirrious.Conference.Core.Models.Raw;

namespace Cirrious.Conference.Core.ViewModels
{
    public class SponsorsViewModel
        : BaseSponsorsViewModel
    {
        public SponsorsViewModel()
        {
            LoadFrom(Service.Sponsors.Values);
        }
    }
}