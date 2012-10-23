namespace Foobar.Dialog.Core.Elements
{
    /// <summary>
    /// Sections contain individual Element instances that are rendered by Android.Dialog
    /// </summary>
    /// <remarks>
    /// Sections are used to group elements in the screen and they are the
    /// only valid direct child of the RootElement. Sections can contain
    /// any of the standard elements, including new RootElements.
    /// 
    /// RootElements embedded in a section are used to navigate to a new
    /// deeper level.
    /// 
    /// You can assign a header and a footer either as strings (Header and Footer)
    /// properties, or as ViewElements to be shown (HeaderView and FooterView).   Internally
    /// this uses the same storage, so you can only show one or the other.
    /// </remarks>
    public interface ISection
    {
        IElement HeaderView { get; set; }
        IElement FooterView { get; set; }
        void Add(IElement element);
    }
}