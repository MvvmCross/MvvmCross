﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
﻿using Cirrious.MvvmCross.Application;
﻿using Cirrious.MvvmCross.Interfaces;
﻿using Cirrious.MvvmCross.Interfaces.ViewModel;
﻿using CustomerManagement.Locators;
﻿using CustomerManagement.ViewModels;

namespace CustomerManagement
{
    public class App 
        : MvxApplication
        , IMvxStartNavigation
    {
        public App()
        {
			// Set the application title
            Title = "Customer Management";

            // Add navigation mappings
            var locators = new List<IMvxViewModelLocator>
                               {
                                   new CustomerListViewModelLocator(), 
                                   new CustomerViewModelLocator()
                               };

            AddLocators(locators);
        }

        public void Start()
        {
            var startViewModel = new StartApplicationObject();
            startViewModel.Start();
        }
    }
}