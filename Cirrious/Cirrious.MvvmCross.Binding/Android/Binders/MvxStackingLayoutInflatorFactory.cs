using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;

namespace Cirrious.MvvmCross.Binding.Android.Binders
{
#warning Kill this file!
    public class MvxStackingLayoutInflatorFactory
        : Java.Lang.Object
          , LayoutInflater.IFactory
    {
        private readonly Stack<LayoutInflater.IFactory> _stack = new Stack<LayoutInflater.IFactory>();

        public View OnCreateView(string name, Context context, IAttributeSet attrs)
        {
            if (_stack.Count == 0)
                return null;

            var topmost = _stack.Peek();
            if (topmost == null)
                return null;

            return topmost.OnCreateView(name, context, attrs);
        }

        public void Push(LayoutInflater.IFactory factory)
        {
            _stack.Push(factory);
        }

        public LayoutInflater.IFactory Pop()
        {
            return _stack.Pop();
        }
    }
}