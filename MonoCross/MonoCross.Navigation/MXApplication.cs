using System;
using System.Collections.Generic;
using System.Linq;
using MonoCross.Navigation.Exceptions;

namespace MonoCross.Navigation
{
    public abstract class MXApplication
    {
        public string NavigateOnLoad { get; set; }
        public string Title { get; set; }
        public NavigationList NavigationMap = new NavigationList();

        protected MXApplication()
        {
            NavigateOnLoad = string.Empty;
        }

        public class NavigationList : List<MXNavigation>
        {
            public void Add(string pattern, IMXController controller)
            {
                this.Add(pattern, controller, new Dictionary<string, string>());
            }

            public IMXController GetControllerForPattern(string pattern)
            {
                return this.Contains(pattern) ? this.Where(m => m.Pattern == pattern).First().Controller : null;
            }

#warning Stuart - should you delete GetPatternForModelType?
            /*
			public String GetPatternForModelType(Type modelType)
			{
				return this.Where(m => m.Controller.ModelType == modelType).First().Pattern;
			}
             */
			
            public bool Contains(string pattern)
            {
                return this.Where(m => m.Pattern == pattern).Count() > 0;
            }

            public void Add(string pattern, IMXController controller, Dictionary<string, string> parameters)
            {
#if DROID
                Android.Util.Log.Debug("NavigationList", "Adding: '" + pattern + "'");
#endif
                // Enforce uniqueness
                MXNavigation currentMatch = this.Where(m => m.Pattern == pattern).FirstOrDefault();
                if (currentMatch != null)
                {
#if DEBUG
                    string text = string.Format("MapUri \"{0}\" is already matched to Controller type {1}",
                                                                            pattern, currentMatch.Controller);
                    throw new MonoCrossException(text);
#else
                    return;
#endif
                }

                this.Add(new MXNavigation(pattern, controller, parameters));
            }
        }
    }
}