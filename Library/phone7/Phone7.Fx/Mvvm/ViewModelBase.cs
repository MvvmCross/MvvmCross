using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Phone7.Fx.Ioc;
using Phone7.Fx.Messaging;
using System.Linq.Expressions;

namespace Phone7.Fx.Mvvm
{
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged //where TView : IView
    {
        //private TView _view;

        private bool _isError;

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        //[Dependency]
        //public TView View
        //{
        //    get { return _view; }
        //    set
        //    {
        //        _view = value;

        //        _view.Loaded += (o, args) => InitalizeData();
        //        //_view.Loaded += (o, args) => { if (_view.NavigationService != null)
        //        //    _view.NavigationService.Navigated += new NavigatedEventHandler(NavigationService_Navigated); };
        //        //_view.LayoutUpdated += new EventHandler(_view_LayoutUpdated);
        //    }
        //}

        //void NavigationService_Navigated(object sender, NavigationEventArgs e)
        //{
        //    _onNavigatedToCalled = true;
        //}

        //void _view_LayoutUpdated(object sender, EventArgs e)
        //{
        //    if (_onNavigatedToCalled)
        //    {
        //        _onNavigatedToCalled = false;
        //        InitalizeData();
        //    }
        //}

        public bool IsError
        {
            get { return _isError; }
            set { if (value != _isError) { _isError = value; RaisePropertyChanged("IsError"); } }
        }

        private bool _isLoading;
        /// <summary>
        /// Gets or sets a value indicating whether the data is loading.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is loading; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoading
        {
            get { return _isLoading; }
            set { if (value != _isLoading) { _isLoading = value; RaisePropertyChanged("IsLoading"); } }
        }


        private bool _isDataLoaded = true;
        /// <summary>
        /// Gets or sets a value indicating when  data are loaded.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if  data are loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { if (value != _isDataLoaded) { _isDataLoaded = value; RaisePropertyChanged("IsDataLoaded"); } }
        }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        //public NavigationService NavigationService
        //{
        //    get
        //    {
        //        return View.NavigationService;
        //    }
        //}

        ///// <summary>
        ///// Gets the navigation context.
        ///// </summary>
        //public NavigationContext NavigationContext
        //{
        //    get { return View.NavigationContext; }
        //}

        /// <summary>
        /// Initalizes the data.
        /// </summary>
        public virtual void InitalizeData()
        {
            Dispatcher.BeginInvoke(() =>
            {
                IsDataLoaded = false;
            });

        }

        public virtual void OnViewReady()
        {

        }

        protected ViewModelBase()
        {

        }

        /// <summary>
        /// Navigato to a xaml view and call ChannelAggregator.Publish method
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="obj"></param>
        /// <param name="urlTo"></param>
        /// <param name="param"></param>
        //public void NavigateTo<TObj, TEvent>(TObj obj, string urlTo, Func<TObj, TEvent> param) where TObj : class
        //{
        //    if (obj != null)
        //    {
        //        NavigatedEventHandler handler = null;
        //        handler = (s, e) =>
        //        {

        //            ChannelAggregator.Publish<TEvent>(param(obj));

        //            if (handler != null)
        //                this.NavigationService.Navigated -= handler;
        //        };
        //        this.NavigationService.Navigated += handler;

        //        this.NavigationService.Navigate(new Uri(urlTo, UriKind.Relative));
        //    }
        //}

        //protected ViewModelBase(TView view)
        //{
        //    this.View = view;
        //}

        private PropertyChangedEventHandler _propertyChanged;


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }


        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        protected void RaisePropertyChanged(string propertyName)
        {
            CheckPropertyName(propertyName);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <typeparam name="TParam">The type of the param.</typeparam>
        /// <param name="property">The property.</param>
        protected void RaisePropertyChanged<TParam>(Expression<Func<TParam>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            string name = memberExpression.Member.Name;
            RaisePropertyChanged(name);
        }


        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (_propertyChanged != null) { _propertyChanged(this, e); }
        }

        /// <summary>
        /// Checks the name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [Conditional("DEBUG")]
        private void CheckPropertyName(string propertyName)
        {
            PropertyInfo p = this.GetType().GetProperty(propertyName);
            if (p == null)
            {
                throw new InvalidOperationException(string.Format(null, "The property with the propertyName '{0}' doesn't exist.", propertyName));
            }
        }
    }
}