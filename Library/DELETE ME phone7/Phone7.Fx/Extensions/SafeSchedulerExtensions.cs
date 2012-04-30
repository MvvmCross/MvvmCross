using System;
using Microsoft.Phone.Reactive;

namespace Phone7.Fx.Extensions
{
    public static class SafeSchedulerExtensions
    {
        public static IScheduler AsSafe(this IScheduler scheduler)
        {
            return new SafeScheduler(scheduler);
        }

        public static IScheduler AsSafe(this IScheduler scheduler, Action<Exception> OnError)
        {
            return new SafeScheduler(scheduler, OnError);
        }

        private class SafeScheduler : IScheduler
        {
            private readonly IScheduler _source;
            private Action<Exception> _onError;

            public SafeScheduler(IScheduler scheduler)
            {
                
            }

            public SafeScheduler(IScheduler scheduler, Action<Exception> OnError)
            {
                // TODO: Complete member initialization
                this._source = scheduler;
                this._onError = OnError;
            }

            public DateTimeOffset Now { get { return _source.Now; } }

            public IDisposable Schedule(Action action, TimeSpan dueTime)
            {
                return _source.Schedule(Wrap(action), dueTime);
            }

            public IDisposable Schedule(Action action)
            {
                return _source.Schedule(Wrap(action));
            }

            private Action Wrap(Action action)
            {
                return () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        // Log and report the exception.
                        if (_onError != null)
                        {
                            _onError(e);
                        }
                    }
                };

            }
        }
    }
}