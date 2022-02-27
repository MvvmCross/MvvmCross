// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Foundation;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    /// <summary>
    /// Mvx user interface refresh control.
    /// http://motzcod.es/post/59125989518/mvxuirefreshcontrol-for-mvvmcross
    /// </summary>
    public class MvxUIRefreshControl : UIRefreshControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MvxUIRefreshControl"/> class.
        /// </summary>
        public MvxUIRefreshControl()
        {
            ValueChanged += OnValueChanged;
        }

        private string _message;

        /// <summary>
        /// Gets or sets the message to display
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value ?? string.Empty;
                AttributedTitle = new NSAttributedString(_message);
            }
        }

        private bool _isRefreshing;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is refreshing.
        /// </summary>
        /// <value><c>true</c> if this instance is refreshing; otherwise, <c>false</c>.</value>
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                if (_isRefreshing)
                    BeginRefreshing();
                else
                    EndRefreshing();
            }
        }

        /// <summary>
        /// Gets or sets the refresh command.
        /// </summary>
        /// <value>The refresh command.</value>
        public ICommand RefreshCommand { get; set; }

        private void OnValueChanged(object sender, EventArgs args)
        {
            ExecuteRefreshCommand(RefreshCommand);
        }

        protected virtual void ExecuteRefreshCommand(ICommand command)
        {
            if (command == null)
                return;

            if (!command.CanExecute(null))
                return;

            command.Execute(null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ValueChanged -= OnValueChanged;
            }

            base.Dispose(disposing);
        }
    }
}
