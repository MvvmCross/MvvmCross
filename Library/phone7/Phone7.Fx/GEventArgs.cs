using System;

namespace Phone7.Fx
{
    public class GEventArgs<T>:EventArgs
    {
        public T Data { get; set; }
    }
}