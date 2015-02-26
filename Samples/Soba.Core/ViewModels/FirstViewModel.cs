using Cirrious.MvvmCross.ViewModels;

namespace Soba.Core.ViewModels
{
	public class FirstViewModel : MvxViewModel
	{
		public FirstViewModel ()
		{
		}

		private string _msg;
		public string Msg {
			get { return _msg; }
			set { 
				if (_msg == value)
					return;
				_msg = value;
				RaisePropertyChanged (() => Msg); 
			}
		}

		private int _value;
		public int Value {
			get { return _value; }
			set { 
				if (_value == value)
					return;
				_value = value;
				RaisePropertyChanged (() => Value); 
			}
		}

		private bool _isOn;
		public bool IsOn {
			get { return _isOn; }
			set { 
				if (_isOn == value)
					return;
				_isOn = value;
				RaisePropertyChanged (() => IsOn); 
			}
		}
	}
}

