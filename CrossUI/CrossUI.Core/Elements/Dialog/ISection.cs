// ISection.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace CrossUI.Core.Elements.Dialog
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
    public interface ISection : IBuildable
    {
        IElement HeaderView { get; set; }
        IElement FooterView { get; set; }

        void Add(IElement element);
    }
}