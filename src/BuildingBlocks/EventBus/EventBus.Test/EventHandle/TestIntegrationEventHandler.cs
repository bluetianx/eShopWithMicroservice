using System;
using System.Threading.Tasks;
using EventBus.Abstractions;
using EventBus.Test.Event;

namespace EventBus.Test.EventHandle
{
    public class TestIntegrationEventHandler : IIntegrationEventHandler<TestEvent>
    {
        public bool Handled { get; private set; }

        public TestIntegrationEventHandler()
        {
            Handled = false;
        }

        public async Task Handle(TestEvent @event)
        {
            Handled = true;
            Console.WriteLine(@event.Msg);
        }
    }
}