#region Copyright

// <copyright file="ISection.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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