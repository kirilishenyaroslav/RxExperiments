using System;
using System.Linq;
using System.Net;
using System.Threading;
using Rx.Services;
using Xunit;

namespace Rx.Tests
{
    public class LiveStoreTests
    {
        [Fact]
        public void Should_Be_Live()
        {
            var data = CreateSampleData();

            var store = new LiveStore<IPAddress>(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                return data;
            });

            Thread.Sleep(TimeSpan.FromSeconds(2));

            Assert.NotEmpty(store);
            Assert.Equal(store.ElementAt(1), IPAddress.Parse("192.168.1.1"));
        }

        [Fact]
        public void Should_Notify()
        {
            var data = CreateSampleData();

            var store = new LiveStore<IPAddress>(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                return data;
            });

            var notified = false;

            Thread.Sleep(TimeSpan.FromSeconds(2));

            store.UpdatedOldSchool += (s, a) => notified = true;

            Assert.True(notified);
        }

        [Fact]
        public void Should_Notify_Rx()
        {
            var data = CreateSampleData();

            var store = new LiveStore<IPAddress>(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                return data;
            });

            var notified = false;

            Thread.Sleep(TimeSpan.FromSeconds(2));

            store.Updated.Subscribe(x => notified = true);

            Thread.Sleep(TimeSpan.FromSeconds(3));

            var notified2 = false;

            store.Updated.Subscribe(x => notified2 = true);

            Assert.True(notified);
            Assert.True(notified2);
        }

        // todo: implement

        static IPAddress[] CreateSampleData() => new[]
        {
            IPAddress.Loopback,
            IPAddress.Parse("192.168.1.1"),
            IPAddress.Parse("109.251.146.196"),
        };
    }
}
