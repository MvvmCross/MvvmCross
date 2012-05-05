#region Copyright
// <copyright file="MvxPssMessagePump.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Pss.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Pss.Views
{
    public class MvxPssMessagePump : IMvxPssCurrentView, IMvxMessagePump
    {
        private readonly MvxPssSystemMessageHandler _systemMessageHandler = new MvxPssSystemMessageHandler();

        #region IMvxPssCurrentView Members

        public IMvxPssView CurrentView { get; set; }

        #endregion

        #region IMvxMessagePump Members

        public void Run()
        {
            System.Console.WriteLine("Run loop starting");
            while (!_systemMessageHandler.ExitFlag)
            {
                System.Console.Write(">");
                var input = System.Console.ReadLine();
                if (CurrentView == null)
                {
                    System.Console.WriteLine("Error - no view shown currently");
                    return;
                }

                if (input == null)
                    input = string.Empty;
                
                if (CurrentView.HandleInput(input))
                    continue;

                if (_systemMessageHandler.HandleInput(CurrentView.ReflectionGetViewModel(), input))
                    continue;

                System.Console.WriteLine("Error - unknown input");
            }
        }

        #endregion
    }
}