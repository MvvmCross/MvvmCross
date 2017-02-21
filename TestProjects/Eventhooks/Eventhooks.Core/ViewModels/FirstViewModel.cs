using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;

namespace Eventhooks.Core.ViewModels
{
	public class FirstViewModel
		: MvxViewModel
	{
		public override void Appearing()
		{
			MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View is appearing");
		}

		public override void Appeared()
		{
			MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View appeared");
		}

		public override void Disappearing()
		{
			MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View is disappearing");
		}

		public override void Disappeared()
		{
			MvxTrace.Trace(MvxTraceLevel.Diagnostic, "View has disappeared");
		}

		public IMvxCommand ShowSecondView 
		{ 
			get { return new MvxCommand(ExecuteSecondViewCommand); } 
		}

		private void ExecuteSecondViewCommand()
		{
			ShowViewModel<SecondViewModel>();
		}
	}
}
