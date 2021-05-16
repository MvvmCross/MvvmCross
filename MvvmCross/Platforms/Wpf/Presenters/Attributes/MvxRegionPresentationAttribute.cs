using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Wpf.Presenters.Attributes
{
    public class MvxRegionPresentationAttribute : MvxBasePresentationAttribute
    {
        public string RegionName { get; set; }
        public string WindowIdentifier { get; set; }
    }
}
