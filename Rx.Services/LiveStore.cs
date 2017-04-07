using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;

namespace Rx.Services
{
    public class LiveStore<T> : IEnumerable<T>
    {
        public LiveStore(Func<IEnumerable<T>> source)
        {
            this.source = source;

            //Updated = notifier.AsObservable();

            ScheduleUpdate();
        }

        public event EventHandler UpdatedOldSchool;

        public IObservable<Unit> Updated => notifier;

        private void ScheduleUpdate()
        {
            (new Thread(() =>
            {
                do
                {
                    data = source();

                    UpdatedOldSchool?.Invoke(this, EventArgs.Empty);

                    notifier.OnNext(Unit.Default);
                }
                while (!exit.WaitOne(TimeSpan.FromSeconds(5)));
            })
            { IsBackground = true, }).Start();
        }

        // todo: implement
        public IEnumerator<T> GetEnumerator()
        {
            var data2 = data;

            if (data2 == null)
            {
                yield break;
            }

            foreach (var item in data2)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private readonly Func<IEnumerable<T>> source;
        private readonly AutoResetEvent exit = new AutoResetEvent(false);
        private readonly ReplaySubject<Unit> notifier = new ReplaySubject<Unit>();

        private volatile IEnumerable<T> data;
    }
}
