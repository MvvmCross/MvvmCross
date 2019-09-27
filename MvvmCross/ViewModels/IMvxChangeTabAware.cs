namespace MvvmCross.ViewModels
{
    public interface IMvxChangeTabAware<TParameter>
    {
        /// <summary>
        /// Get Data After Tab Changed
        /// </summary>
        /// <param name="parameter">data prepared from previous tab</param>
        void OnNavigatedTo(TParameter parameter);
        /// <summary>
        /// Return Data before Changing the Tab
        /// </summary>
        /// <returns>parameter prepared for next tab</returns>
        TParameter OnNavigatedFrom();
    }
}
