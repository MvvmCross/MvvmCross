namespace MvvmCross.iOS.Support.JASidePanels
{
    using SidePanels;
    using iOS.Views;
    using iOS.Views.Presenters;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform;

    using Core.ViewModels;
    using Core.Views;
    using System.Linq;
    using UIKit;
    using System;

    /// <summary>
    /// A presenter that uses the JASidePanels component to allow 3 panels for view presentation
    /// and also master detail split view presentation when required
    /// </summary>
    /// <seealso cref="MvvmCross.iOS.Views.Presenters.MvxIosViewPresenter" />
    public class MvxSidePanelsPresenter : MvxIosViewPresenter
    {
        #region Private fields

        /// <summary>
        /// The _ja side panel controller
        /// </summary>
        private readonly MvxMultiPanelController _multiPanelController;

        /// <summary>
        /// The _active panel
        /// </summary>
        private MvxPanelEnum _activePanel;

        /// <summary>
        /// The _current modal view controller
        /// </summary>
        private UIViewController _currentModalViewController;

        /// <summary>
        /// The _splitview controller
        /// </summary>
        private MvxBaseSplitViewController _splitviewController;

        #endregion Private fields

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxSidePanelsPresenter"/> class.
        /// </summary>
        /// <param name="applicationDelegate">The application delegate.</param>
        /// <param name="window">The window.</param>
        public MvxSidePanelsPresenter(UIApplicationDelegate applicationDelegate, UIWindow window) :
            base(applicationDelegate, window)
        {
            _multiPanelController = new MvxMultiPanelController();
            _activePanel = MvxPanelEnum.Center;

            Mvx.RegisterSingleton<IMvxSideMenu>(_multiPanelController);
        }

        #endregion ctors

        #region Private Properties

        /// <summary>
        /// Gets the get active panel UI navigation controller.
        /// </summary>
        /// <value>
        /// The get active panel UI navigation controller.
        /// </value>
        private UINavigationController GetActivePanelUiNavigationController
        {
            get
            {
                switch (_activePanel)
                {
                    case MvxPanelEnum.Center:
                        return CentrePanelUiNavigationController();

                    case MvxPanelEnum.Left:
                        return LeftPanelUiNavigationController();

                    case MvxPanelEnum.Right:
                        return RightPanelUiNavigationController();
                }

                return CentrePanelUiNavigationController();
            }
        }

        #endregion Private Properties

        #region Private Methods

        /// <summary>
        /// Centres the panel UI navigation controller.
        /// </summary>
        /// <returns></returns>
        private UINavigationController CentrePanelUiNavigationController()
        {
            return ((UINavigationController)_multiPanelController.CenterPanel);
        }

        /// <summary>
        /// Rights the panel UI navigation controller.
        /// </summary>
        /// <returns></returns>
        private UINavigationController RightPanelUiNavigationController()
        {
            return ((UINavigationController)_multiPanelController.RightPanel);
        }

        /// <summary>
        /// Lefts the panel UI navigation controller.
        /// </summary>
        /// <returns></returns>
        private UINavigationController LeftPanelUiNavigationController()
        {
            return ((UINavigationController)_multiPanelController.LeftPanel);
        }

        /// <summary>
        /// Processes the active panel presentation.
        /// </summary>
        /// <param name="hint">The hint.</param>
        private void ProcessActivePanelPresentation(MvxPresentationHint hint)
        {
            var activePresentationHint = hint as MvxActivePanelPresentationHint;
            if (activePresentationHint != null)
            {
                var panelHint = activePresentationHint;

                _activePanel = panelHint.ActivePanel;

                if (panelHint.ShowPanel)
                {
                    ShowPanel(panelHint.ActivePanel);
                }
            }
        }

        /// <summary>
        /// Processes the pop to root presentation.
        /// </summary>
        /// <param name="hint">The hint.</param>
        private void ProcessPopToRootPresentation(MvxPresentationHint hint)
        {
            var popHint = hint as MvxPanelPopToRootPresentationHint;
            if (popHint != null)
            {
                var panelHint = popHint;

                switch (panelHint.Panel)
                {
                    case MvxPanelEnum.Center:
                        if (CentrePanelUiNavigationController() != null)
                        {
                            CentrePanelUiNavigationController().PopToRootViewController(false);
                        }
                        break;

                    case MvxPanelEnum.Left:
                        if (LeftPanelUiNavigationController() != null)
                        {
                            LeftPanelUiNavigationController().PopToRootViewController(false);
                        }
                        break;

                    case MvxPanelEnum.Right:
                        if (RightPanelUiNavigationController() != null)
                        {
                            RightPanelUiNavigationController().PopToRootViewController(false);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Processes the reset root presentation.
        /// </summary>
        /// <param name="hint">The hint.</param>
        private void ProcessResetRootPresentation(MvxPresentationHint hint)
        {
            var popHint = hint as MvxPanelResetRootPresentationHint;
            if (popHint != null)
            {
                var panelHint = popHint;
                UINavigationController navController;
                switch (panelHint.Panel)
                {
                    case MvxPanelEnum.Center:
                        navController = _multiPanelController.CenterPanel as UINavigationController;
                        if (navController != null)
                        {
                            navController.ViewControllers = new UIViewController[0];
                        }
                        else
                        {
                            _multiPanelController.CenterPanel = null;
                        }
                        break;

                    case MvxPanelEnum.Left:
                        navController = _multiPanelController.LeftPanel as UINavigationController;
                        if (navController != null)
                        {
                            navController.ViewControllers = new UIViewController[0];
                        }
                        else
                        {
                            _multiPanelController.LeftPanel = null;
                        }
                        break;

                    case MvxPanelEnum.Right:
                        navController = _multiPanelController.RightPanel as UINavigationController;
                        if (navController != null)
                        {
                            navController.ViewControllers = new UIViewController[0];
                        }
                        else
                        {
                            _multiPanelController.RightPanel = null;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Shows the panel.
        /// </summary>
        /// <param name="panel">The panel.</param>
        private void ShowPanel(MvxPanelEnum panel)
        {
            switch (panel)
            {
                case MvxPanelEnum.Center:
                    _multiPanelController.ShowCenterPanelAnimated(true);
                    break;

                case MvxPanelEnum.Left:
                    _multiPanelController.ShowLeftPanelAnimated(true);
                    break;

                case MvxPanelEnum.Right:
                    _multiPanelController.ShowRightPanelAnimated(true);
                    break;
            }
        }

        /// <summary>
        /// See if the supplied ViewModel matches up with the MvxView at the top of the supplied UINavigationController
        /// and if so, pop that View from the stack
        /// </summary>
        private bool CloseTopView(IMvxViewModel toClose, UINavigationController uiNavigationController)
        {
            if (uiNavigationController == null)
            {
                return false;
            }

            var mvxTouchView = uiNavigationController.TopViewController as IMvxIosView;

            if (mvxTouchView == null)
            {
                return false;
            }

            if (mvxTouchView.ReflectionGetViewModel() != toClose)
            {
                return false;
            }

            uiNavigationController.PopViewController(true);

            return true;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Shows the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <exception cref="MvxException">
        /// Only one modal view controller at a time supported
        /// or
        /// Passed in IMvxTouchView is not a UIViewController
        /// </exception>
        public override void Show(IMvxIosView view)
        {
            // Handle modal first
            // This will use our TopLevel UINavigation Controller, to present over the top of the Panels UX
            if (view is IMvxModalIosView)
            {
                if (_currentModalViewController != null)
                {
                    throw new MvxException("Only one modal view controller at a time supported");
                }

                _currentModalViewController = view as UIViewController;
                PresentModalViewController(view as UIViewController, true);
                return;
            }

            // Then handle panels
            var viewController = view as UIViewController;
            if (viewController == null)
            {
                throw new MvxException("Passed in IMvxTouchView is not a UIViewController");
            }

            if (MasterNavigationController == null)
            {
                ShowFirstView(viewController);
            }
            else
            {
                // here we need to get the Presentation Panel attribute details
                var panelAttribute = viewController.GetType().GetCustomAttributes(typeof(MvxPanelPresentationAttribute), true).FirstOrDefault() as MvxPanelPresentationAttribute;
                if (panelAttribute != null)
                {
                    // this section of presentation code deals with showing master/detail views on
                    // large screen devices (i.e. iPads)
                    if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                    {
                        if (panelAttribute.SplitViewBehaviour == MvxSplitViewBehaviour.Master)
                        {
                            var splitHost = new MvxSplitViewControllerHost();
                            _splitviewController = new MvxBaseSplitViewController();
                            _splitviewController.SetLeft(new UINavigationController((UIViewController)view));
                            splitHost.DisplayContentController(_splitviewController);
                            viewController = splitHost;
                        }
                        else if (panelAttribute.SplitViewBehaviour == MvxSplitViewBehaviour.Detail && _splitviewController != null)
                        {
                            _splitviewController.SetRight(new UINavigationController((UIViewController)view));
                            // since we have now shown the view simply return
                            return;
                        }
                    }

                    switch (panelAttribute.HintType)
                    {
                        case MvxPanelHintType.ActivePanel:
                            ChangePresentation(new MvxActivePanelPresentationHint(panelAttribute.Panel, panelAttribute.ShowPanel));
                            break;

                        case MvxPanelHintType.PopToRoot:
                            ChangePresentation(new MvxPanelPopToRootPresentationHint(panelAttribute.Panel));
                            break;

                        case MvxPanelHintType.ResetRoot:
                            ChangePresentation(new MvxPanelResetRootPresentationHint(panelAttribute.Panel));
                            break;
                    }
                }

                if (GetActivePanelUiNavigationController == null)
                {
                    // If we have cleared down our panel completely, then we will be setting a new root view
                    // this is perfect for Menu items
                    switch (_activePanel)
                    {
                        case MvxPanelEnum.Center:
                            _multiPanelController.CenterPanel = new UINavigationController(viewController);
                            break;

                        case MvxPanelEnum.Left:
                            _multiPanelController.LeftPanel = new UINavigationController(viewController);
                            break;

                        case MvxPanelEnum.Right:
                            _multiPanelController.RightPanel = new UINavigationController(viewController);
                            break;
                    }
                }
                else
                {
                    //figure out which panel we were just asked to show, left, right, center and properly place it
                    switch (panelAttribute.Panel)
                    {
                        case MvxPanelEnum.Center:
                            if (null == _multiPanelController.CenterPanel)
                            {
                                _multiPanelController.CenterPanel = new UINavigationController(viewController);
                            }
                            else
                            {
                                CentrePanelUiNavigationController().PushViewController(viewController, true);
                            }
                            break;
                        case MvxPanelEnum.Left:
                            if (null == _multiPanelController.LeftPanel)
                            {
                                _multiPanelController.LeftPanel = new UINavigationController(viewController);
                            }
                            else
                            {
                                LeftPanelUiNavigationController().PushViewController(viewController, true);
                            }
                            break;
                        case MvxPanelEnum.Right:
                            if (null == _multiPanelController.RightPanel)
                            {
                                _multiPanelController.RightPanel = new UINavigationController(viewController);
                            }
                            else
                            {
                                RightPanelUiNavigationController().PushViewController(viewController, true);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Natives the modal view controller disappeared on its own.
        /// </summary>
        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            if (_currentModalViewController != null)
            {
                MvxTrace.Error("How did a modal disappear when we didn't have one showing?");
            }
            else
            {
                _currentModalViewController = null;
            }
        }

        /// <summary>
        /// Closes the modal view controller.
        /// </summary>
        public override void CloseModalViewController()
        {
            if (_currentModalViewController != null)
            {
                _currentModalViewController.DismissModalViewController(false);
                _currentModalViewController = null;
            }
            else
            {
                base.CloseModalViewController();
            }
        }

        /// <summary>
        /// Changes the presentation.
        /// </summary>
        /// <param name="hint">The hint.</param>
        public override void ChangePresentation(MvxPresentationHint hint)
        {
            ProcessActivePanelPresentation(hint);
            ProcessResetRootPresentation(hint);
            ProcessPopToRootPresentation(hint);

            base.ChangePresentation(hint);
        }

        /// <summary>
        /// Closes the specified to close.
        /// </summary>
        /// <param name="toClose">To close.</param>
        public override void Close(IMvxViewModel toClose)
        {
            if (_currentModalViewController != null)
            {
                var mvxTouchView = _currentModalViewController as IMvxIosView;
                if (mvxTouchView == null)
                {
                    MvxTrace.Error("Unable to close view - modal is showing but not an IMvxTouchView");
                }
                else if (mvxTouchView.ReflectionGetViewModel() != toClose)
                {
                    MvxTrace.Error("Unable to close view - modal is showing but is not the requested viewmodel");
                }
                else
                {
                    _currentModalViewController.DismissViewController(true, () => { });
                    _currentModalViewController = null;
                }

                return;
            }

            // We will look across all active navigation stacks to see if we can
            // pop our MvxView associated with this MvxViewModel (saves explicitly having to specify)
            var modelClosed = CloseTopView(toClose, CentrePanelUiNavigationController());
            if (!modelClosed)
            {
                modelClosed = CloseTopView(toClose, LeftPanelUiNavigationController());
            }
            if (!modelClosed)
            {
                modelClosed = CloseTopView(toClose, RightPanelUiNavigationController());
            }

            if (!modelClosed)
            {
                MvxTrace.Warning("Don't know how to close this viewmodel - none of topmost views represent this viewmodel");
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Shows the first view.
        /// </summary>
        /// <param name="viewController">The view controller.</param>
        protected override void ShowFirstView(UIViewController viewController)
        {
            base.ShowFirstView(viewController);

            // So lets push our JaSidePanels viewController and then our first viewController in the centre panel to start things off
            // We will let our initial viewmodel load up the panels as required
            MasterNavigationController.NavigationBarHidden = true;
            MasterNavigationController.PushViewController(_multiPanelController, false);
            _multiPanelController.CenterPanel = new UINavigationController(viewController);
        }

        #endregion Protected Methods
    }
}