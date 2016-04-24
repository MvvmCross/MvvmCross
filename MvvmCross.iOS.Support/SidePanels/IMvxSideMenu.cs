namespace MvvmCross.iOS.Support.SidePanels
{
    public interface IMvxSideMenu
    {
        /// <summary>
        /// Closes the active menu, if none are open nothing will happen.
        /// When multiple are open, all will close.
        /// </summary>
        void Close();
    }
}

