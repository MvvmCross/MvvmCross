using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
  public class MvxUISearchBarSelectedScopeButtonIndexTargetBinding : MvxPropertyInfoTargetBinding<UISearchBar>
  {
    public MvxUISearchBarSelectedScopeButtonIndexTargetBinding(object target, PropertyInfo targetPropertyInfo)
      : base(target, targetPropertyInfo)
    {

      var searchBar = View;
      if (searchBar == null)
      {
        MvxBindingTrace.Trace(MvxTraceLevel.Error,
                       "Error - UISearchBar is null in MvxUISearchBarSelectedScopeButtonIndexTargetBinding");

      }
      else
      {
        searchBar.SelectedScopeButtonIndexChanged += HandleSearchBarValueChanged;
      }

    }

    private void HandleSearchBarValueChanged(object sender, System.EventArgs e)
    {
      FireValueChanged(View.SelectedScopeButtonIndex);
    }

    public override MvxBindingMode DefaultMode
    {
      get { return MvxBindingMode.TwoWay; }
    }

    protected override void Dispose(bool isDisposing)
    {
      base.Dispose(isDisposing);
      if (isDisposing)
      {
        var searchBar = View;
        if (searchBar != null)
        {
          searchBar.SelectedScopeButtonIndexChanged -= HandleSearchBarValueChanged;
        }
      }
    }

  }
}