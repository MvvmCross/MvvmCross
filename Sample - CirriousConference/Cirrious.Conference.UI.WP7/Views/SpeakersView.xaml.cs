using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.Conference.Core.ViewModels.SessionLists;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.Conference.UI.WP7.Views
{
    public abstract class BaseSpeakersView : BaseView<SpeakersViewModel>
    {        
    }

    public partial class SpeakersView : BaseSpeakersView
    {
        public SpeakersView()
        {
            InitializeComponent();
        }
    }
}