using Cirrious.Conference.Core.Models;

namespace Cirrious.Conference.Core.ViewModels
{
    public class ExhibitionViewModel
        : BaseSponsorsViewModel
    {
        public ExhibitionViewModel()
        {
            LoadFrom(Service.Exhibitors.Values);
        }
    }
}