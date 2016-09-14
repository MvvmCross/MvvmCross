namespace MvvmCross.iOS.Support.SidePanels
{
    public interface IMvxSideMenu
    {
        /// <summary>
        /// Closes the active menu, if none are open nothing will happen.
        /// When multiple are open, all will close.
        /// </summary>
        void Close();

        /// <summary>
        /// Opens the left or the right menu (as indicated by the parameter).
        /// </summary>
        /// <param name="panelEnum">Indicates if either the left or the right menu should be opened.</param>
        void Open(MvxPanelEnum panelEnum);
    }
}