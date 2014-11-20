using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Views.Animation;
using Cirrious.MvvmCross.WindowsCommon.Views.Handlers;
using Cirrious.MvvmCross.WindowsCommon.Views.Suspension;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    public delegate void NavigatedEventHandler(object sender, MvxWindowsNavigationEventArgs e);

    /// <summary>Navigation container for pages. </summary>
    public class MvxWindowsFrame : Control, INavigate
    {
        private int _currentIndex = -1;
        private IPageAnimation _pageAnimation;
        private List<MvxWindowsPageDescription> _pages = new List<MvxWindowsPageDescription>();

        /// <summary>Gets the current <see cref="MtFrame"/>. </summary>
        public static MvxWindowsFrame Current
        {
            get { return Window.Current.Content as MvxWindowsFrame; }
        }

        /// <summary>Gets the current page index. </summary>
        public int CurrentIndex
        {
            get { return _currentIndex; }
        }

        /// <summary>Gets a value indicating whether the first/root page is visible. </summary>
        public bool IsFirstPage
        {
            get { return _currentIndex == 0; }
        }

        private IMvxSuspensionManager _suspensionManager;
        protected IMvxSuspensionManager SuspensionManager
        {
            get
            {
                _suspensionManager = _suspensionManager ?? Mvx.Resolve<IMvxSuspensionManager>();
                return _suspensionManager;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="MtFrame"/> class. </summary>
        public MvxWindowsFrame()
        {
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;

            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            Loaded += delegate { Window.Current.VisibilityChanged += OnVisibilityChanged; };
            Unloaded += delegate { Window.Current.VisibilityChanged -= OnVisibilityChanged; };

            DefaultStyleKey = typeof(MvxWindowsFrame);

            if (NavigationKeyHandler.IsRunningOnPhone)
            {
                DisableForwardStack = true;
                _pageAnimation = new TurnstilePageAnimation();
            }
            else
                DisableForwardStack = false;
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(MvxWindowsFrame), new PropertyMetadata(default(object)));

        /// <summary>
        /// Gets or sets the content of the <see cref="MtFrame"/>. 
        /// </summary>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        //public static readonly DependencyProperty ContentTransitionsProperty = DependencyProperty.Register(
        //    "ContentTransitions", typeof(TransitionCollection), typeof(MtFrame), new PropertyMetadata(default(TransitionCollection), OnContentTransitionsChanged));

        //private static void OnContentTransitionsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        //{
        //    if (((TransitionCollection)args.NewValue).Any(t => t.GetType().Name == "NavigationThemeTransition"))
        //        throw new Exception("ContentTransitions collection cannot contain NavigationThemeTransition, use MtFrame.PageAnimation for page transition animations. ");

        //    ((MtFrame)obj).InternalFrame.ContentTransitions = (TransitionCollection)args.NewValue;
        //}

        public static readonly DependencyProperty ContentTransitionsProperty = DependencyProperty.Register(
            "ContentTransitions", typeof(TransitionCollection), typeof(MvxWindowsFrame), new PropertyMetadata(default(TransitionCollection)));

        /// <summary>Gets or sets the content transitions of the <see cref="MtFrame"/>. </summary>
        public TransitionCollection ContentTransitions
        {
            get { return (TransitionCollection)GetValue(ContentTransitionsProperty); }
            set { SetValue(ContentTransitionsProperty, value); }
        }

        /// <summary>Occurs when navigating to a page. </summary>
        public event NavigatedEventHandler Navigated;

        /// <summary>Gets a command to navigate to the previous page. </summary>
        private ICommand _goBackCommand;
        public ICommand GoBackCommand
        {
            get { return _goBackCommand ?? (_goBackCommand = new MvxCommand(() => GoBackAsync(), () => CanGoBack)); }
        }

        /// <summary>Gets or sets a value indicating whether the forward stack is disabled 
        /// (default: disabled on Windows Phone, enabled on Windows). </summary>
        public bool DisableForwardStack { get; set; }

        /// <summary>Gets or sets a value indicating whether the cache is fully 
        /// deactivated (should be used only for testing). Default: false. </summary>
        public bool DisableCache { get; set; }

        /// <summary>Gets the page before the current page in the page stack or null if not available. </summary>
        public MvxWindowsPageDescription PreviousPage { get { return _currentIndex > 0 ? _pages[_currentIndex - 1] : null; } }

        /// <summary>Gets the current page. </summary>
        public MvxWindowsPageDescription CurrentPage { get { return _pages.Count > 0 ? _pages[_currentIndex] : null; } }

        /// <summary>Gets the page after the current page in the page stack or null if not available. </summary>
        public MvxWindowsPageDescription NextPage { get { return _currentIndex < _pages.Count - 1 ? _pages[_currentIndex + 1] : null; } }

        /// <summary>Gets the current page animation. Only available when ContentTransitions is null. </summary>
        public IPageAnimation PageAnimation
        {
            get { return ContentTransitions == null ? _pageAnimation : null; }
            set { _pageAnimation = value; }
        }

        /// <summary>Gets or sets a value indicating whether to show the animation when launching, leaving or switching to the app. Default: false. </summary>
        public bool ShowNavigationOnAppInAndOut { get; set; }

        /// <summary>Gets the underlying WinRT frame object. </summary>
        public Frame InternalFrame { get; private set; }

        /// <summary>Gets a value indicating whether it is possible to navigate forward. </summary>
        public bool CanGoForward { get { return _currentIndex < _pages.Count - 1; } }

        /// <summary>Tries to navigate forward to the next page. </summary>
        /// <returns>Returns true if navigating forward, false if cancelled</returns>
        public async Task<bool> GoForwardAsync()
        {
            if (await CallOnNavigatingFromAsync(CurrentPage, NavigationMode.Forward))
                return false;

            GoForwardOrBack(NavigationMode.Forward);
            return true;
        }

        /// <summary>Gets a value indicating whether it is possible to navigate back. </summary>
        public bool CanGoBack { get { return _currentIndex > 0; } }

        /// <summary>Gets a list of the pages in the page stack. </summary>
        public IReadOnlyList<MvxWindowsPageDescription> Pages { get { return _pages; } }

        /// <summary>Gets the first page of the specified type in the page back stack or null if no page of the type is available. </summary>
        /// <param name="pageType">The page type. </param>
        /// <returns>The page or null if not found. </returns>
        public MvxWindowsPageDescription GetNearestPageOfTypeInBackStack(Type pageType)
        {
            var index = _currentIndex;
            while (index >= 0)
            {
                if (_pages[index].Type == pageType)
                    return _pages[index];
                index--;
            }
            return null;
        }

        /// <summary>Navigates back to the given page. </summary>
        /// <param name="page">The page to navigate to. </param>
        /// <returns>True if the navigation could be performed. </returns>
        public async Task<bool> GoBackToAsync(MvxWindowsPageDescription page)
        {
            var index = _pages.IndexOf(page);
            return await GoBackToAsync(index);
        }

        /// <summary>Navigates back to the given index. </summary>
        /// <param name="newIndex">The page index. </param>
        /// <returns>True if the navigation could be performed. </returns>
        public async Task<bool> GoBackToAsync(int newIndex)
        {
            if (newIndex == _currentIndex)
                return true;

            if (newIndex < 0 || newIndex > _currentIndex)
                return false;

            while (_currentIndex != newIndex)
            {
                if (!await GoBackAsync())
                    return false;
            }

            return true;
        }

        /// <summary>Navigates back to the first page in the page stack. </summary>
        /// <returns>True if the navigation could be performed. </returns>
        public async Task<bool> GoHomeAsync()
        {
            while (_currentIndex != 0)
            {
                if (!await GoBackAsync())
                    return false;
            }
            return true;
        }

        /// <summary>Gets a value indicating whether the frame is currently navigating to another page. </summary>
        public bool IsNavigating { get; private set; }

        /// <summary>Tries to navigate back to the previous page. </summary>
        /// <returns>Returns true if navigating back, false if cancelled or CanGoBack is false. </returns>
        public async Task<bool> GoBackAsync()
        {
            if (await CallOnNavigatingFromAsync(CurrentPage, NavigationMode.Back))
                return false;

            GoForwardOrBack(NavigationMode.Back);
            return true;
        }

        private void GoForwardOrBack(NavigationMode mode)
        {
            if (mode == NavigationMode.Forward ? CanGoForward : CanGoBack)
            {
                var oldPage = CurrentPage;
                _currentIndex += mode == NavigationMode.Forward ? 1 : -1;
                var newPage = CurrentPage;

                if (mode == NavigationMode.Back && DisableForwardStack)
                    RemoveAllPagesAfterCurrent();

                Content = newPage.GetPage(this).InternalPage;

                CallOnNavigatedFrom(oldPage, mode);
                CallOnNavigatedTo(newPage, mode);

                ((MvxCommand)GoBackCommand).RaiseCanExecuteChanged();
            }
            else
                throw new Exception("cannot go forward or back");
        }

        /// <summary>Initializes the frame and navigates to the given first page. </summary>
        /// <param name="homePageType">The type of the home page. </param>
        /// <param name="parameter">The parameter for the page. </param>
        /// <returns>Always true. </returns>
        public bool Initialize(Type homePageType, object parameter = null)
        {
            NavigateInternal(homePageType, parameter);
            return true;
        }

        /// <summary>Navigates forward to a new instance of the given page type. </summary>
        /// <param name="pageType">The page type. </param>
        /// <returns>Returns true if the navigation process has not been cancelled. </returns>
        public Task<bool> NavigateAsync(Type pageType)
        {
            return NavigateAsync(pageType, null);
        }

        /// <summary>Navigates forward to a new instance of the given page type. </summary>
        /// <param name="pageType">The page type. </param>
        /// <param name="parameter">The page parameter. </param>
        /// <returns>Returns true if the navigation process has not been cancelled. </returns>
        public async Task<bool> NavigateAsync(Type pageType, object parameter)
        {
            if (CurrentPage != null && await CallOnNavigatingFromAsync(CurrentPage, NavigationMode.New))
                return false;

            NavigateInternal(pageType, parameter);
            return true;
        }

        /// <summary>Navigates to the page of the given type. </summary>
        /// <param name="sourcePageType">The page type. </param>
        public bool Navigate(Type sourcePageType)
        {
            NavigateAsync(sourcePageType);
            return true;
        }

        /// <summary>Navigates to the page of the given type. </summary>
        /// <param name="sourcePageType">The page type. </param>
        public bool Navigate(Type sourcePageType, object parameter)
        {
            NavigateAsync(sourcePageType, parameter);
            return true;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InternalFrame = (Frame)GetTemplateChild("Frame");
        }

        private void OnVisibilityChanged(object sender, VisibilityChangedEventArgs args)
        {
            if (CurrentPage != null)
                CurrentPage.GetPage(this).OnVisibilityChanged(args);
        }

        private void RemoveAllPagesAfterCurrent()
        {
            for (var i = _pages.Count - 1; i > _currentIndex; i--)
            {
                var page = _pages[i];
                _pages.Remove(page);
            }
        }

        private void NavigateInternal(Type type, object parameter)
        {
            // Remove forward stack
            var previousPage = CurrentPage;
            RemoveAllPagesAfterCurrent();

            // Create new page
            var newPage = new MvxWindowsPageDescription(type, parameter);
            _pages.Add(newPage);
            _currentIndex++;

            Content = newPage.GetPage(this).InternalPage;

            // Call navigation event methods
            if (previousPage != null)
                CallOnNavigatedFrom(previousPage, NavigationMode.New);
            CallOnNavigatedTo(newPage, NavigationMode.New);

            ((MvxCommand)GoBackCommand).RaiseCanExecuteChanged();

            // Destroy current page if cache is disabled
            if (previousPage != null && (previousPage.Page.NavigationCacheMode == NavigationCacheMode.Disabled || DisableCache))
                previousPage.Page = null;
        }

        private void CallOnNavigatedFrom(MvxWindowsPageDescription description, NavigationMode mode)
        {
            var page = description.GetPage(this);
            var args = new MvxWindowsNavigationEventArgs();
            args.Content = page;
            args.SourcePageType = description.Type;
            args.Parameter = description.Parameter;
            args.NavigationMode = mode;
            page.InternalOnNavigatedFrom(args);
        }

        private async Task<bool> CallOnNavigatingFromAsync(MvxWindowsPageDescription description, NavigationMode mode)
        {
            var page = description.GetPage(this);
            var args = new MvxWindowsNavigatingCancelEventArgs();
            args.Content = page;
            args.SourcePageType = description.Type;
            args.NavigationMode = mode;

            IsNavigating = true;
            await page.InternalOnNavigatingFromAsync(args);
            IsNavigating = false;

            return args.Cancel;
        }

        private void CallOnNavigatedTo(MvxWindowsPageDescription description, NavigationMode mode)
        {
            var page = description.GetPage(this);
            var args = new MvxWindowsNavigationEventArgs();
            args.Content = page;
            args.SourcePageType = description.Type;
            args.Parameter = description.Parameter;
            args.NavigationMode = mode;
            page.InternalOnNavigatedTo(args);

            var copy = Navigated;
            if (copy != null)
                copy(this, args);

            OnNavigated(this, args);

            if (args.NavigationMode == NavigationMode.New)
                OnPageCreated(this, page);
        }

        /// <summary>Called when a new page has been created. </summary>
        /// <param name="sender">The frame. </param>
        /// <param name="page">The created page. </param>
        protected virtual void OnPageCreated(object sender, object page)
        {
            // Must be empty. 
        }

        /// <summary>Called when navigated to another page. </summary>
        /// <param name="sender">The sender. </param>
        /// <param name="args">The args. </param>
        protected virtual void OnNavigated(object sender, MvxWindowsNavigationEventArgs args)
        {
            // Must be empty. 
        }

        /// <summary>Used set the serialized the current page stack (used in the SuspensionManager). </summary>
        /// <param name="data">The data. </param>
        public void SetNavigationState(string data)
        {
            using (var xmlReader = XmlReader.Create(new StringReader(data)))
            {
                var dataContractSerializer = new DataContractSerializer(typeof(MvxWindowsFrameDescription),
                SuspensionManager.KnownTypes.ToArray());

                var frameDescription = (MvxWindowsFrameDescription)dataContractSerializer.ReadObject(xmlReader);
                _pages = frameDescription.PageStack;
                _currentIndex = frameDescription.PageIndex;

                if (_currentIndex != -1)
                {
                    Content = CurrentPage.GetPage(this).InternalPage;
                    CallOnNavigatedTo(CurrentPage, NavigationMode.Back);
                }
            }
        }

        /// <summary>Used to serialize the current page stack (used in the SuspensionManager). </summary>
        /// <returns>The data. </returns>
        public string GetNavigationState()
        {
            //CallOnNavigatingFromAsync(CurrentPage, NavigationMode.Forward);
            CallOnNavigatedFrom(CurrentPage, NavigationMode.Forward);

            // remove pages which do not support tombstoning
            var pagesToSerialize = _pages;
            var currentIndexToSerialize = _currentIndex;
            var firstPageToRemove = _pages.FirstOrDefault(p =>
            {
                var page = p.GetPage(this);
                return !page.IsSuspendable;
            });

            if (firstPageToRemove != null)
            {
                var index = pagesToSerialize.IndexOf(firstPageToRemove);
                pagesToSerialize = _pages.Take(index).ToList();
                currentIndexToSerialize = index - 1;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    var serializer = new DataContractSerializer(typeof(MvxWindowsFrameDescription), SuspensionManager.KnownTypes.ToArray());
                    serializer.WriteObject(memoryStream, new MvxWindowsFrameDescription { PageIndex = currentIndexToSerialize, PageStack = pagesToSerialize });
                    memoryStream.Position = 0;
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>Gets the number of pages in the page back stack. </summary>
        public int BackStackDepth
        {
            get { return _currentIndex + 1; }
        }
    }
}