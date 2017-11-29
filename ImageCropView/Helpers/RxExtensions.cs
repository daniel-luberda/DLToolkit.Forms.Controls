using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DLToolkit.Forms.Controls
{
    internal static class RxExtensions
    {
        public static IObservable<T> ThrottleResponsive<T>(this IObservable<T> source, IObservable<TimeSpan> delay)
        {
            return source.Publish(src =>
            {
                var fire = new Subject<T>();

                var whenCanFire = fire
                    .Select(u => new Unit())
                    .Delay(v => delay)
                    .StartWith(new Unit());

                IDisposable subscription = src
                    .CombineVeryLatest(whenCanFire, (x, flag) => x)
                    .Subscribe(fire);

                return fire.Finally(subscription.Dispose);
            });
        }

        public static IObservable<TResult> CombineVeryLatest
          <TLeft, TRight, TResult>(this IObservable<TLeft> leftSource,
          IObservable<TRight> rightSource, Func<TLeft, TRight, TResult> selector)
        {
            return Observable.Defer(() =>
            {
                int l = -1, r = -1;
                return Observable.CombineLatest(
                    leftSource.Select(Tuple.Create<TLeft, int>),
                    rightSource.Select(Tuple.Create<TRight, int>),
                        (x, y) => new { x, y })
                    .Where(t => t.x.Item2 != l && t.y.Item2 != r)
                    .Do(t => { l = t.x.Item2; r = t.y.Item2; })
                    .Select(t => selector(t.x.Item1, t.y.Item1));
            });
        }
    }
}
