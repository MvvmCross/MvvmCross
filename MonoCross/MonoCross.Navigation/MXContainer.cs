using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using MonoCross.Navigation.Exceptions;

namespace MonoCross.Navigation
{
    public static class MXNavigationExtensions
    {
        public static void Navigate(this IMXView view, string url)
        {
            MXContainer.Navigate(view, url);
        }

        public static void Navigate(this IMXView view, string url, Dictionary<string, string> parameters)
        {
            MXContainer.Navigate(view, url, parameters);
        }

        public static void Navigate(this IMXView view, string urlBase, params object[] items)
        {
            view.Navigate(string.Format(urlBase, items));
        }
    }

    public interface IMXContainer
    {
        void ShowPerspective(IMXView fromView, IMXController controller, string perspective);
        void Redirect(string url);
        void ShowError(IMXView fromView, IMXController controller, Exception exception);
    }

    public abstract class MXContainer : IMXContainer
    {
        public DateTime LastNavigationDate { get; set; }

        public string LastNavigationUrl { get; set; }

		/// <summary>
		/// The cancel load.
		/// </summary>
        protected bool CancelLoad = false;

        /// <summary>
        /// 
        /// </summary>
        public bool ThreadedLoad = true;

		/// <summary>
		/// Raises the controller load begin event.
		/// </summary>
		protected virtual void OnControllerLoadBegin(IMXController controller)
		{
		}

		/// <summary>
		/// Raises the controller load failed event.
		/// </summary>
		protected virtual void OnControllerLoadFailed(IMXController controller, Exception ex)
		{
		}

		/// <summary>
		/// Raises the load complete event after the Controller has completed loading its Model, the View may be populated
		/// and the derived classs should check if it exists and do something with it if needed for the platform, either free it,
		/// pop off the views in a stack above it or whatever makes sense to the platform  
		/// </summary>
		/// <param name='controller'>
		/// Controller.
		/// </param>
		/// <param name='viewPerspective'>
		/// View perspective.
		/// </param>
		protected abstract void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective);
        private MXViewMap _views = new MXViewMap();
        public virtual MXViewMap Views
        {
            get { return _views; }
        }

        // ctor/abstract singleton initializers

        // disallow explict construction of the container by non-derived classes
        protected MXContainer(MXApplication theApp)
        {
            _theApp = theApp;
        }

        /// <summary>
        /// Initializes the specified target factory instance.
        /// </summary>
        /// <param name="newInstance">A <see cref="T"/> representing the target factory value.</param>
        /// 
        /*
        protected static void Initialize(MXContainer newInstance)
        {
            if (newInstance == null)
                throw new ArgumentNullException();
            Instance = newInstance;
        }
        */

        public MXApplication App
        {
            get { return _theApp; }
        }
        private MXApplication _theApp;

        public delegate String SessionIdDelegate();
        static protected SessionIdDelegate GetSessionId;

        protected static void InitializeContainer(MXContainer theContainer)
        {
            string sessionId = string.Empty;
            if (GetSessionId != null)
                sessionId = GetSessionId();

            instances[sessionId] = theContainer;
        }

        public static MXContainer Instance
        { 
            get
            {
                string sessionId = string.Empty;
                if (GetSessionId != null)
                    sessionId = GetSessionId();
                MXContainer toReturn;
                if (instances.TryGetValue(sessionId, out toReturn))
                    return toReturn;
                return null;
            }
        }
        // kept in a map for use in server environments (WebKit)
        private static Dictionary<string, MXContainer> instances = new Dictionary<string, MXContainer>();

		// Model to View associations
        public static void AddView<Model>(IMXView view)
        {
            Instance.AddView(new MXViewPerspective(typeof(Model), ViewPerspective.Default), view.GetType(), view);
        }
        public static void AddView<Model>(IMXView view, string perspective)
        {
            Instance.AddView(new MXViewPerspective(typeof(Model), perspective), view.GetType(), view);
        }
        public static void AddView<Model>(Type viewType)
        {
            Instance.AddView(new MXViewPerspective(typeof(Model), ViewPerspective.Default), viewType, null);
        }
        public static void AddView<Model>(Type viewType, string perspective)
        {
            Instance.AddView(new MXViewPerspective(typeof(Model), perspective), viewType, null);
        }
        protected virtual void AddView(Type modeltype, Type viewType, string perspective)
        {
            Instance.AddView(new MXViewPerspective(modeltype, perspective), viewType, null);
        }
        protected virtual void AddView(MXViewPerspective viewPerspective, Type viewType, IMXView view)
        {
            if (view == null)
                Views.Add(viewPerspective, viewType);
            else
                Views.Add(viewPerspective, view);
        }  

		// 
        public static MXNavigation MatchUrl(string url)
        {
            return Instance.App.NavigationMap.Where(pattern => Regex.Match(url, pattern.RegexPattern()).Value == url).FirstOrDefault();
        }

        public static void Navigate(string url)
        {
            InternalNavigate(null, url, new Dictionary<string, string>());
        }

        public static void Navigate(IMXView view, string url)
        {
			InternalNavigate(view, url, new Dictionary<string, string>());
		}

        public static void Navigate(IMXView view, string url, Dictionary<string, string> parameters)
        {
			InternalNavigate(view, url, parameters);
		}

        private static void InternalNavigate(IMXView fromView, string url, Dictionary<string, string> parameters)
		{
            MXContainer container = Instance;   // optimization for the server size, property reference is a hashed lookup

			// fetch and allocate a viable controller
            IMXController controller = container.GetController(url, parameters);
            // Initiate load for the associated controller passing all parameters
            if (controller != null)
            {
                container.OnControllerLoadBegin(controller);

                container.CancelLoad = false;

				// synchronize load layer to prevent collisions on web-based targets.
                lock (container)
	            {
					// Console.WriteLine("InternalNavigate: Locked");
					
	                // if there is no synchronization, don't launch a new thread
                    if (container.ThreadedLoad)
					{
	                    // new thread to execute the Load() method for the layer
	                    new Thread((object args) =>
	                    {
							try
							{
                                //Dictionary<string, string> parameters = (Dictionary<string, string>)args;
                                container.LoadController(fromView, controller, parameters);
							}
							catch (Exception ex)
							{
                                container.OnControllerLoadFailed(controller, ex);
							}
								
						}).Start(parameters);
					}
					else
					{
						try
						{
                            container.LoadController(fromView, controller, parameters);
						}
						catch (Exception ex)
						{
                            container.OnControllerLoadFailed(controller, ex);
						}
					}
	
					// Console.WriteLine("InternalNavigate: Unlocking");
				}
			}
		}

		/*
		void LoadController(IMXView fromView, IMXController controller, Dictionary<string, string> parameters)
		{
            string perspective = controller.Load(parameters);
			
			// HACKHACK - the CancelLoad hack from MonoCross team is just horrid so I've added a magic string to it :/
            if (perspective != "Ignore" && !Instance.CancelLoad) // done if failed
            {
                MXViewPerspective viewPerspective = new MXViewPerspective(controller.ModelType, perspective);
				// quick check (viable for ALL platforms) to see if there is some kind of a mapping set up
                if (!Instance.Views.ContainsKey(viewPerspective))
					throw new Exception("There is no View mapped for " + viewPerspective.ToString());

				// if we have a view lying around assign it from the map, more of a curtesy to the derived container that anything
				controller.View = Instance.Views.GetView(viewPerspective);
				if (controller.View != null) 
					controller.View.SetModel(controller.GetModel());

				// give the derived container the ability to do something
				// with the fromView if it exists or to create it if it doesn't
				Instance.OnControllerLoadComplete(fromView, controller, viewPerspective);
            }
			// clear CancelLoad, we're done
            Instance.CancelLoad = false;
		}
        */

        void LoadController(IMXView fromView, IMXController controller, Dictionary<string, string> parameters)
        {
            var action = controller.Load(parameters);
            action.Perform(this, fromView, controller);
        }

        public virtual void ShowPerspective(IMXView fromView, IMXController controller, string perspective)
        {
            MXViewPerspective viewPerspective = new MXViewPerspective(controller.ModelType, perspective);
            // quick check (viable for ALL platforms) to see if there is some kind of a mapping set up
            if (!Instance.Views.ContainsKey(viewPerspective))
                throw new MonoCrossException("There is no View mapped for " + viewPerspective.ToString());

            // if we have a view lying around assign it from the map, more of a curtesy to the derived container that anything
            controller.View = Instance.Views.GetView(viewPerspective);
            if (controller.View != null)
                controller.View.SetModel(controller.GetModel());

            // give the derived container the ability to do something
            // with the fromView if it exists or to create it if it doesn't
            Instance.OnControllerLoadComplete(fromView, controller, viewPerspective);
        }

        public virtual void ShowError(IMXView fromView, IMXController controller, Exception exception)
        {
            // default implementation is to just wrap and throw the exception
            throw exception.MXWrap();
        }

	    public abstract void Redirect(string url);

        public virtual IMXController GetController(string url, Dictionary<string, string> parameters)
        {
            IMXController controller = null;
            MXNavigation navigation = null;

            // return if no url provided
            if (url == null)
                throw new ArgumentException("url is NULL");

            // set last navigation
            LastNavigationDate = DateTime.Now;
            LastNavigationUrl = url;

            // initialize parameter dictionary if not provided
            parameters = (parameters ?? new Dictionary<string, string>());

            // for debug
            // Console.WriteLine("Navigating to: " + url);

            // get map object
            navigation = MatchUrl(url);

            // If there is no result, assume the URL is external and create a new Browser View
            if (navigation != null)
            {
                controller = navigation.Controller;

                // Now that we know which mapping the URL matches, determine the parameter names for any Values in URL string
                Match match = Regex.Match(url, navigation.RegexPattern());
                MatchCollection args = Regex.Matches(navigation.Pattern, @"{(?<Name>\w+)}*");

                // If there are any parameters in the URL string, add them to the parameters dictionary
                if (match != null && args.Count > 0)
                {
                    foreach (Match arg in args)
                    {
                        if (parameters.ContainsKey(arg.Groups["Name"].Value))
                            parameters.Remove(arg.Groups["Name"].Value);
                        parameters.Add(arg.Groups["Name"].Value, match.Groups[arg.Groups["Name"].Value].Value);
                    }
                }

                //Add default view parameters without overwriting current ones new comment here
                if (navigation.Parameters.Count > 0)
                {
                    foreach (KeyValuePair<string, string> param in navigation.Parameters)
                    {
                        if (!parameters.ContainsKey(param.Key))
                        {
                            parameters.Add(param.Key, param.Value);
                        }
                    }
                }
            }
            else
			{
#if DEBUG
				throw new Exception("URI match not found for: " + url);
#else
				// should log the message at least
#endif
			}

            controller.Uri = url;
            controller.Parameters = parameters;

            return controller;
        }

        public static void RenderViewFromPerspective(IMXController controller, MXViewPerspective perspective)
        {
            Instance.Views.RenderView(perspective, controller.GetModel());
        }
		
        public class MXViewMap
        {
			Dictionary<MXViewPerspective, IMXView> _viewMap = new Dictionary<MXViewPerspective, IMXView>();
			Dictionary<MXViewPerspective, Type> _typeMap = new Dictionary<MXViewPerspective, Type>();
            
            public void Add(MXViewPerspective perspective, Type viewType)
            {
                if (!viewType.GetInterfaces().Contains(typeof(IMXView)))
                    throw new ArgumentException("Type provided does not implement IMXView interface.", "viewType");
                _typeMap.Add(perspective, viewType);
				_viewMap.Add(perspective, null);
				
				
				MXViewPerspective vp = new MXViewPerspective(perspective.ModelType, perspective.Perspective);
				System.Diagnostics.Debug.Assert(_typeMap.ContainsKey(vp));
				System.Diagnostics.Debug.Assert(_viewMap.ContainsKey(vp));
            }
			
            public void Add(MXViewPerspective perspective, IMXView view)
            {
				_viewMap.Add(perspective, view);
				_typeMap.Add(perspective, view.GetType());
            }
            
            public Type GetViewType(MXViewPerspective viewPerspective) 
            {
                Type type;
                _typeMap.TryGetValue(viewPerspective, out type);
                return type;
            }
			
            public IMXView GetView(MXViewPerspective viewPerspective)
            {
                IMXView view = null;
                _viewMap.TryGetValue(viewPerspective, out view);
                return view;
            }

			public IMXView GetOrCreateView(MXViewPerspective viewPerspective)
			{
				IMXView view = null;
				if (_viewMap.TryGetValue(viewPerspective, out view))
				{
					// if we have a type registered and haven't yet created an instance, this will be null
					if (view != null)
						return view;
				}
				Type viewType = null;
				if (_typeMap.TryGetValue(viewPerspective, out viewType)) {
                    // Instantiate an instance of the view from it's type
                    view = (IMXView)Activator.CreateInstance(viewType);
					// add to the map for later.
					_viewMap[viewPerspective] = view;
				} else {
					// No view
					throw new ArgumentException("No View Perspective found for: " + viewPerspective.ToString(), "viewPerspective");
				}
				return view;
			}
			
			public bool ContainsKey(MXViewPerspective viewPerspective)
			{
				return _viewMap.ContainsKey(viewPerspective);
			}
            
			public MXViewPerspective GetViewPerspectiveForViewType(Type viewType)
			{
				return _typeMap.First( keyValuePair => keyValuePair.Value == viewType ).Key;
			}
			
            internal void RenderView(MXViewPerspective viewPerspective, object model)
            {
                IMXView view = GetOrCreateView(viewPerspective);
				if (view == null)
				{
					// No view perspective found for model
						throw new ArgumentException("No View Perspective found for: " + viewPerspective.ToString(), "viewPerspective");
				}
                view.SetModel(model);
                view.Render();
            }
        }
    }
}