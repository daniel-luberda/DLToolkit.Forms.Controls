using System;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
    internal class IntervalThrottle
    {
        readonly Action _onStart;
        readonly Action _onInterval;
        readonly Action _onFinish;
        readonly object _lock = new object();

        bool _isThrottling;
        int _count;

        public IntervalThrottle(int initialDelay, Action onStart, Action onInterval, Action onFinish)
        {
            Delay = initialDelay;
            this._onFinish = onFinish;
            this._onInterval = onInterval;
            this._onStart = onStart;
        }

        public int Delay { get; set; }

        public void Handle()
        {
            lock (_lock)
            {
                InternalHandle();
            }
        }

        void InternalHandle()
        {
            _count = _count + 1;

            if (!_isThrottling)
            {
                _isThrottling = true;
                _count = 0;
                RunTimer();
            }
        }

        async void RunTimer()
        {
            _onStart.Invoke();
            int current;

            while (_isThrottling)
            {
                current = _count;
                await Task.Delay(Delay);

                if (current == _count)
                {
                    _isThrottling = false;
                }
                else
                {
                    _onInterval.Invoke();
                }
            }

            await Task.Delay(Delay);
            _onFinish?.Invoke();
        }
    }
}
